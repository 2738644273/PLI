using PLI.Attributes;
using PLI.DBManipulation;

namespace PLI.SqlExecutors.Executors
{ /// <summary>
    /// 新增策略
    /// </summary>
    public class InsertExecutor:SqlExecutorStrategy
    {
        public override object Executor(AbstractDbInvoker actionInvoker,object[] arguments)
        {
            if (actionInvoker.SqlStatementAttribute.GetType() == typeof(InsertAttribute))
            {
                IsSuccess = true;
                if (IsTaskOfAnyType(actionInvoker.SqlDescriptor.ReturnType))
                {
                    return actionInvoker.InsertAsync(arguments);
                }
                return actionInvoker.Insert(arguments);

            }
            
            return null;
        }
        
    }
}