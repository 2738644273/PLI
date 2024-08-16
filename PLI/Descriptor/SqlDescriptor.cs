using System;
using System.Collections.Generic;
using System.Reflection;

namespace PLI.Descriptor
{
    /// <summary>
    /// SQL描述器
    /// </summary>
    public class SqlDescriptor
    {
        /// <summary>
        /// sql信息
        /// </summary>
        public string Sql { get; set; }
        
        /// <summary>
        /// 返回类型
        /// </summary>
        public Type ReturnType { get; set; }
        
        /// <summary>
        /// 实现方法
        /// </summary>
        public MethodInfo Method { get; set; }
        
        
        /// <summary>
        /// Sql参数集合
        /// </summary>
        public List<SqlParameterDescriptor> ParameterDescriptors { get; set; }
        
        
    }
}