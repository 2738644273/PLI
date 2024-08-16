using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using GrEmit;
using PLI.Attributes;
using PLI.DBManipulation;
using PLI.DBManipulation.Default;
using PLI.ILEmit;
using PLI.Providers;
using PLI.Utils;

namespace PLI
{
    /// <summary>
    /// 接口实现器
    /// </summary>
    public class RepositoryFactory
    {
        public RepositoryFactory(IDbManipulationInterceptor interceptor, DescriptorProvider descriptorProvider, Type dbInvokerType,object dbClient)
        {
            _interceptor = interceptor;
            _descriptorProvider = descriptorProvider;
            _dbInvokerType = dbInvokerType;
            _dbClient = dbClient;
        }

        private readonly IDbManipulationInterceptor _interceptor;
        private readonly DescriptorProvider _descriptorProvider;
        private readonly Type _dbInvokerType;
        private readonly object _dbClient;
        /// <summary>
        /// 动态创建实现接口类
        /// </summary>
        /// <param name="typeName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public  T CreateRepository<T>() 
            where T : class
           
         
        {
            var proxyType = CreateProxyType(typeof(T));
            var CtorFunc = LambdaUtil.CreateCtorFunc<IDbManipulationInterceptor, AbstractDbInvoker[], T>(proxyType);
            var dataBaseInvokers = _descriptorProvider.CreateAbstractDbInvoker(_dbInvokerType,_dbClient,typeof(T));
            return CtorFunc.Invoke(_interceptor, dataBaseInvokers);
        }

        
        
        /// <summary>
        /// IDbManipulationInterceptor的执行方法
        /// </summary>
        private static readonly MethodInfo interceptMethod = typeof(IDbManipulationInterceptor).GetMethod(nameof(IDbManipulationInterceptor.ExecuteCommand)) ?? throw new MissingMethodException(nameof(IDbManipulationInterceptor.ExecuteCommand));
        /// <summary>
        /// 代理类型的构造器的参数类型  方便依赖注入
        /// (IDbManipulationInterceptor interceptor,AbstractDbInvoker[] dbInvokers)
        /// </summary>
        private static readonly Type[] proxyTypeCtorArgTypes ={typeof(IDbManipulationInterceptor), typeof(AbstractDbInvoker[])};

        /// <summary>
        /// 动态创建实现接口类
        /// </summary>
        /// <param name="typeName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Type CreateProxyType(params Type[] @interfaces)
        {
            //创建动态模块
            var moduleBuilder = ILEmitAssmbly.CreateModuleBuilder();
            // 创建动态类
            var typeBuilder = ILEmitType.CreateTypeBuilder(moduleBuilder, @interfaces);
            var fieldInterceptor = ILEmitType.BuildField(typeBuilder, "<>apiInterceptor", typeof(IDbManipulationInterceptor));
            var fieldInvokers = ILEmitType.BuildField(typeBuilder, "<>actionInvokers", typeof(AbstractDbInvoker[]));
            //创建动态构造
            ILEmitType.BuildCtor(typeBuilder,proxyTypeCtorArgTypes,fieldInterceptor,fieldInvokers);
            //平铺所有待实现接口方法
            var publicMethods =
                @interfaces.SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance)).ToArray();

            // 实现所有公开的实例方法
            ImplMethod(typeBuilder,publicMethods,fieldInterceptor,fieldInvokers);
            // 构建类型
            Type dynamicType = typeBuilder.CreateTypeInfo();
            return dynamicType;
        }

        /// <summary>
        /// 实现接口方法
        /// </summary>
        /// <param name="moduleBuilder"></param>
        /// <param name="typeBuilder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static void ImplMethod(TypeBuilder typeBuilder,MethodInfo[] publicMethods,  FieldBuilder fieldInterceptor,
            FieldBuilder fieldInvokers)
        {
            //获取所有待实现接口
            var allIRepositorys = typeBuilder.GetInterfaces()
                .Where(p => p.GetCustomAttributes<RepositoryAttribute>().Count() > 0);
            if (allIRepositorys.Count() == 0)
            {
                throw new Exception($"没有任何标注[{nameof(RepositoryAttribute)}]特性接口");
            }
          
          
            ILEmitType.BuildMethods(typeBuilder,publicMethods,fieldInterceptor,fieldInvokers,interceptMethod);
        }
    }
}