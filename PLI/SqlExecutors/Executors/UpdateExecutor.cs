using PLI.Attributes;
using PLI.DBManipulation;

namespace PLI.SqlExecutors.Executors
{
    /// <summary>
    /// 修改策略
    /// </summary>
    public class UpdateExecutor : SqlExecutorStrategy
    {
        public override object Executor(AbstractDbInvoker actionInvoker, object[] arguments)
        {
            if (actionInvoker.SqlStatementAttribute.GetType() == typeof(UpdateAttribute))
            {
                IsSuccess = true;
                if (IsTaskOfAnyType(actionInvoker.SqlDescriptor.ReturnType))
                {
                    return actionInvoker.UpdateAsync(arguments);
                }

                return actionInvoker.Update(arguments);
            }

            return null;
        }
    }
}