using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace PLI.ILEmit
{
    /// <summary>
    /// IL类型操作
    /// </summary>
    public class ILEmitType
    {
        /// <summary>
        /// 模块前缀名
        /// </summary>
        private const string Moduleprefix = "PLI_";

        /// <summary>
        /// 创建方法绑定器
        /// </summary>
        public static MethodBuilder CreateEmitMethod(TypeBuilder typeBuilder, MethodInfo method)
        {
            //创建方法实现
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                method.ReturnType,
                method.GetParameters().Select(p => p.ParameterType).ToArray()
            );
            return methodBuilder;
        }

        /// <summary>
        /// 创建类型绑定器
        /// </summary>
        /// <param name="moduleBuilder"></param>
        /// <param name="interfaces"></param>
        /// <returns></returns>
        public static TypeBuilder CreateTypeBuilder(ModuleBuilder moduleBuilder, params Type[] @interfaces)
        {
            string typeName = Moduleprefix + Guid.NewGuid().ToString("N");
            var builder = moduleBuilder.DefineType(typeName, System.Reflection.TypeAttributes.Class);
            foreach (var interfaceType in @interfaces)
            {
                builder.AddInterfaceImplementation(interfaceType);
            }

            return builder;
        }

        /// <summary>
        /// 生成代理类型的字段
        /// </summary>
        /// <param name="builder">类型生成器</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldType">字段类型</param>
        /// <returns></returns>
        public static FieldBuilder BuildField(TypeBuilder builder, string fieldName, Type fieldType)
        {
            const FieldAttributes filedAttribute = FieldAttributes.Private | FieldAttributes.InitOnly;
            return builder.DefineField(fieldName, fieldType, filedAttribute);
        }

        /// <summary>
        /// 生成代理类型的构造器
        /// </summary>
        /// <param name="builder">类型生成器</param>
        /// <param name="fieldApiInterceptor">拦截器字段</param>
        /// <param name="fieldActionInvokers">action执行器字段</param> 
        /// <returns></returns>
        public static void BuildCtor(TypeBuilder builder, Type[] proxyTypeCtorArgTypes,
            FieldBuilder fieldInterceptor,
            FieldBuilder fieldInvokers)
        {
            // .ctor(IHttpApiInterceptor apiInterceptor, ApiActionInvoker[] actionInvokers)
            var ctor = builder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard,
                proxyTypeCtorArgTypes);

            var il = ctor.GetILGenerator();

            // this.apiInterceptor = 第一个参数
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, fieldInterceptor);

            // this.actionInvokers = 第二个参数
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Stfld, fieldInvokers);

            il.Emit(OpCodes.Ret);
        }

        public static void BuildMethods(TypeBuilder builder, MethodInfo[] actionMethods,
            FieldBuilder fieldInterceptor, FieldBuilder fieldInvokers,MethodInfo interceptMethod)
        {
            // private final hidebysig newslot virtual
            const MethodAttributes implementAttribute = MethodAttributes.Private | MethodAttributes.Final |
                                                        MethodAttributes.HideBySig | MethodAttributes.NewSlot |
                                                        MethodAttributes.Virtual;

            for (var i = 0; i < actionMethods.Length; i++)
            {
                var actionMethod = actionMethods[i];
                var actionParameters = actionMethod.GetParameters();
                var parameterTypes = actionParameters.Select(p => p.ParameterType).ToArray();
                var actionMethodName = $"{actionMethod.DeclaringType?.FullName}.{actionMethod.Name}";

                var methodBuilder = builder.DefineMethod(actionMethodName, implementAttribute,
                    CallingConventions.Standard | CallingConventions.HasThis, actionMethod.ReturnType, parameterTypes);
                builder.DefineMethodOverride(methodBuilder, actionMethod);
                var iL = methodBuilder.GetILGenerator();

                // this.apiInterceptor
                iL.Emit(OpCodes.Ldarg_0);
                iL.Emit(OpCodes.Ldfld, fieldInterceptor);

                // this.actionInvokers[i]
                iL.Emit(OpCodes.Ldarg_0);
                iL.Emit(OpCodes.Ldfld, fieldInvokers);
                iL.Emit(OpCodes.Ldc_I4, i);
                iL.Emit(OpCodes.Ldelem_Ref);

                // var arguments = new object[parameters.Length]
                var arguments = iL.DeclareLocal(typeof(object[]));
                iL.Emit(OpCodes.Ldc_I4, actionParameters.Length);
                iL.Emit(OpCodes.Newarr, typeof(object));
                iL.Emit(OpCodes.Stloc, arguments);

                for (var j = 0; j < actionParameters.Length; j++)
                {
                    iL.Emit(OpCodes.Ldloc, arguments);
                    iL.Emit(OpCodes.Ldc_I4, j);
                    iL.Emit(OpCodes.Ldarg, j + 1);

                    var parameterType = parameterTypes[j];
                    if (parameterType.IsValueType || parameterType.IsGenericParameter)
                    {
                        iL.Emit(OpCodes.Box, parameterType);
                    }

                    iL.Emit(OpCodes.Stelem_Ref);
                }

                // 加载 arguments 参数
                iL.Emit(OpCodes.Ldloc, arguments);

                // Intercept(actionInvoker, arguments)
                iL.Emit(OpCodes.Callvirt, interceptMethod);

                if (actionMethod.ReturnType == typeof(void))
                {
                    iL.Emit(OpCodes.Pop);
                }

                iL.Emit(OpCodes.Castclass, actionMethod.ReturnType);
                iL.Emit(OpCodes.Ret);
            }
        }
    }
}