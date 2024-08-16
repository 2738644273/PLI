using System;
using System.Threading.Tasks;
using PLI.Attributes;
using PLI.SqlExecutors;

namespace PLI.DBManipulation.Default
{
    /// <summary>
    /// 默认数据库拦截
    /// </summary>
    public class DefaultDbInterceptor : IDbManipulationInterceptor
    {
        public object ExecuteCommand(AbstractDbInvoker actionInvoker, object[] arguments)
        {
            return SqlExecutor.Executor(actionInvoker,arguments);
        }
    }
}