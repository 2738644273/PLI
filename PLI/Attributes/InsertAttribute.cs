namespace PLI.Attributes
{
    /// <summary>
    /// 新增
    /// </summary>
    public class InsertAttribute:SqlStatementAttribute
    {
        public InsertAttribute(string sqlStatement) : base(sqlStatement)
        {
        }
    }
}