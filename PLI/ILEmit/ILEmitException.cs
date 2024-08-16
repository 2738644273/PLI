using System;
using System.Reflection;
using System.Reflection.Emit;
using GrEmit;

namespace PLI.ILEmit
{
    /// <summary>
    /// 实现异常类方法
    /// </summary>
    public class ILEmitException
    {
        /// <summary>
        ///  绑定未实现异常
        /// </summary>
        /// <param name="methodBuilder"></param>
        /// <param name="excepitonType"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void BindNotImplExcepiton(TypeBuilder typeBuilder,MethodInfo method,Type excepitonType)
        {
            if (!(excepitonType.IsSubclassOf(typeof(Exception))))
            {
                throw new ArgumentException($"excepitonType 类型必须是{nameof(Exception)}的子类");
            }

           var methodBuilder =  ILEmitType.CreateEmitMethod(typeBuilder, method);
           typeBuilder.DefineMethodOverride(methodBuilder, method);
            using (var il = new GroboIL(methodBuilder))
            {
             
                il.Newobj(excepitonType.GetConstructor(Type.EmptyTypes));
                il.Throw();
            }
            
        }
    }
}