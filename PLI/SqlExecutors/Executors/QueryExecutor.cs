using PLI.Attributes;
using PLI.DBManipulation;

namespace PLI.SqlExecutors.Executors
{
    /// <summary>
    /// 查询执行
    /// </summary>
    public class QueryExecutor:SqlExecutorStrategy
    {
        public override object Executor(AbstractDbInvoker actionInvoker,object[] arguments)
        {
            if (actionInvoker.SqlStatementAttribute.GetType() == typeof(QueryAttribute))
            {
                IsSuccess = true;
                if (IsTaskOfAnyType(actionInvoker.SqlDescriptor.ReturnType))
                {
                    return actionInvoker.QueryAsync(arguments);
                }
                return actionInvoker.Query(arguments);
            }
                return null;
        }
    }
}