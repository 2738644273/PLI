namespace PLI.Attributes
{
    /// <summary>
    /// 删除
    /// </summary>
    public class DeleteAttribute:SqlStatementAttribute
    {
        public DeleteAttribute(string sqlStatement) : base(sqlStatement)
        {
        }
    }
}