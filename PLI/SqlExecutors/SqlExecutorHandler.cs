using System;
using System.Threading.Tasks;
using PLI.DBManipulation;

namespace PLI.SqlExecutors
{
    /// <summary>
    /// SQL执行器策略
    /// </summary>
    public abstract class SqlExecutorStrategy
    {
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 执行策略
        /// </summary>
        /// <param name="actionInvoker"></param>
        /// <returns></returns>
        public abstract object Executor(AbstractDbInvoker actionInvoker,object[] arguments);
        
        /// <summary>
        /// 是否Task<>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected  bool IsTaskOfAnyType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
        }
    }
}