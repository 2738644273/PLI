using System;

namespace PLI.Attributes
{
    /// <summary>
    /// 查询语句抽象特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class SqlStatementAttribute:Attribute
    {
        public string SqlStatement { get; set; }

        public SqlStatementAttribute(string sqlStatement)
        {
            SqlStatement = sqlStatement;
        }
    }
}