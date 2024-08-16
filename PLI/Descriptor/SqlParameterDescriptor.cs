using System;

namespace PLI.Descriptor
{
    /// <summary>
    /// SQL参数描述
    /// </summary>
    public class SqlParameterDescriptor
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 返回类型
        /// </summary>
        public Type ParameterType { get; set; }
        /// <summary>
        /// 参数索引
        /// </summary>
        public int ParameterIndex { get; set; }
    }
}