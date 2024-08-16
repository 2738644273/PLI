using System.Threading.Tasks;
using PLI.Attributes;
using PLI.Descriptor;

namespace PLI.DBManipulation
{
    /// <summary>
    /// 抽象数据库执行
    /// </summary>
    public abstract class AbstractDbInvoker
    {
        public object DbClient { get; }
        public AbstractDbInvoker(object sqlclient)
        {
            DbClient = sqlclient;
        }
        public SqlStatementAttribute SqlStatementAttribute { get; set; }
        public SqlDescriptor SqlDescriptor { get; set; }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public abstract object  Query(object[] arguments);
        /// <summary>
        /// 异步查询
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public abstract object  QueryAsync(object[] arguments);

        public abstract object Delete(object[] arguments);
        
        public abstract object DeleteAsync(object[] arguments);
        
        public abstract object Update(object[] arguments);
        
        public abstract object UpdateAsync(object[] arguments);
        
        public abstract object Insert(object[] arguments);
        
        public abstract object InsertAsync(object[] arguments);
    }
}