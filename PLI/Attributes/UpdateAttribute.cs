namespace PLI.Attributes
{
    /// <summary>
    /// 更新
    /// </summary>
    public class UpdateAttribute:SqlStatementAttribute
    {
        public UpdateAttribute(string sqlStatement) : base(sqlStatement)
        {
        }
    }
}