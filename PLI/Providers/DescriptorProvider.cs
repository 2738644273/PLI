using System;
using PLI.DBManipulation;
using PLI.DBManipulation.Default;

namespace PLI.Providers
{
    /// <summary>
    /// 提供Sql描述器
    /// </summary>
    public abstract class DescriptorProvider
    {
        public abstract AbstractDbInvoker[] CreateAbstractDbInvoker(Type dbInvokerType,object dbClient, params Type[] interfaces);
    }
}