using System;
using System.Reflection;
using System.Reflection.Emit;

namespace PLI.ILEmit
{
    /// <summary>
    /// Emit 程序集操作
    /// </summary>
    public class ILEmitAssmbly
    {
        /// <summary>
        /// 创建程序集模块绑定器
        /// </summary>
        /// <param name="asmName"></param>
        /// <param name="moduleName"></param>
        public static ModuleBuilder CreateModuleBuilder()
        {
            var moduleName = Guid.NewGuid().ToString("N");
            var assemblyName = new AssemblyName(Guid.NewGuid().ToString("N"));

            var module = AssemblyBuilder
                .DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName);
            return module;
        }
    }
}