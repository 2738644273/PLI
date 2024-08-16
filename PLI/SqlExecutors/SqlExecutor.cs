using System;
using System.Collections.Generic;
using PLI.DBManipulation;
using PLI.SqlExecutors.Executors;

namespace PLI.SqlExecutors
{
    /// <summary>
    /// 执行器
    /// </summary>
    public class SqlExecutor
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="actionInvoker"></param>
        /// <returns></returns>
        public static object Executor(AbstractDbInvoker actionInvoker,object[] arguments)
        {
            List<SqlExecutorStrategy> sqlExecutorStrategies = new List<SqlExecutorStrategy>()
            {
                new DeleteExecutor(),
                new InsertExecutor(),
                new UpdateExecutor(),
                new QueryExecutor()
            };
            foreach (var sqlExecutor in sqlExecutorStrategies)
            {
               var obj =  sqlExecutor.Executor(actionInvoker,arguments);
               if (sqlExecutor.IsSuccess)
               {
                   return obj;
               }
            }

            return null;
        }
    }
}