using System.Threading.Tasks;

namespace PLI.DBManipulation.Default
{
    public class DefaultDbInvoker:AbstractDbInvoker
    {
 
        public override object Query(object[] arguments)
        {
            throw new System.NotImplementedException("Query暂未实现");
        }

        public override object QueryAsync(object[] arguments)
        {
            throw new System.NotImplementedException("QueryAsync暂未实现");
        }

        public override object Delete(object[] arguments)
        {
            throw new System.NotImplementedException("Delete暂未实现");
        }

        public override object DeleteAsync(object[] arguments)
        {
            throw new System.NotImplementedException("DeleteAsync暂未实现");
        }

        public override object Update(object[] arguments)
        {
            throw new System.NotImplementedException("Update暂未实现");
        }

        public override object UpdateAsync(object[] arguments)
        {
            throw new System.NotImplementedException("UpdateAsync暂未实现");
        }

        public override object Insert(object[] arguments)
        {
            throw new System.NotImplementedException("Insert暂未实现");
        }

        public override object InsertAsync(object[] arguments)
        {
            throw new System.NotImplementedException("InsertAsync暂未实现");
        }

        public DefaultDbInvoker(object sqlclient) : base(sqlclient)
        {
        }
    }
}