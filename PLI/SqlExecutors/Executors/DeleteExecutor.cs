using PLI.Attributes;
using PLI.DBManipulation;

namespace PLI.SqlExecutors.Executors
{
    /// <summary>
    /// 删除策略
    /// </summary>
    public class DeleteExecutor:SqlExecutorStrategy
    {
        public override object Executor(AbstractDbInvoker actionInvoker,object[] arguments)
        {
            if (actionInvoker.SqlStatementAttribute.GetType() == typeof(DeleteAttribute))
            {  
                IsSuccess = true;
                if (IsTaskOfAnyType(actionInvoker.SqlDescriptor.ReturnType))
                {
                    return actionInvoker.DeleteAsync(arguments);
                }
                return actionInvoker.Delete(arguments);
            }
            return null;
        }
        
    }
 
}