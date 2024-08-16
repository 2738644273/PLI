namespace PLI.Attributes
{
    /// <summary>
    /// Sql查询特性
    /// </summary>
    public class QueryAttribute:SqlStatementAttribute
    {
        public QueryAttribute(string sqlStatement) : base(sqlStatement)
        {
            
        }
    }
}

