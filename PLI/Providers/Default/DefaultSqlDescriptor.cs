using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using PLI.Attributes;
using PLI.DBManipulation;
using PLI.DBManipulation.Default;
using PLI.Descriptor;

namespace PLI.Providers.Default
{
    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultSqlDescriptor:DescriptorProvider
    {
             /// <summary>
        /// 生成参数描述对象
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="mi"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
          List<SqlParameterDescriptor> GenerateSqlParameterDescriptors(MethodInfo methodInfo,List<string> sqlParams, List<ParameterInfo> methodParams )
        {
            List<SqlParameterDescriptor> paramDescriptors = new List<SqlParameterDescriptor>();
            // 将 SQL 参数与方法参数进行绑定
            for (int i = 0; i < sqlParams.Count; i++)
            {
                string sqlParamName = sqlParams[i];
                ParameterInfo matchingMethodParam = methodParams.FirstOrDefault(p => p.Name.ToLower() == sqlParamName.Substring(1).ToLower());
              var paramIndex =   Array.IndexOf(methodInfo.GetParameters(), matchingMethodParam);
                if (matchingMethodParam != null)
                {
                    SqlParameterDescriptor descriptor = new SqlParameterDescriptor
                    {
                        Name = sqlParamName,
                        ParameterType = matchingMethodParam.ParameterType,
                        ParameterIndex =  paramIndex 
                    };
                    paramDescriptors.Add(descriptor);
                }
                else
                {
                    throw new Exception($"SQL parameter '{sqlParamName}' does not have a matching method parameter.");
                }
            }

            return paramDescriptors;
        }
         public   override AbstractDbInvoker[] CreateAbstractDbInvoker(Type dbInvokerType,object dbClient,params Type[] interfaces)
        {
            if (!(dbInvokerType.BaseType == typeof(AbstractDbInvoker)))
            {
                throw new ArgumentException($"{dbInvokerType.Name}不是{nameof(AbstractDbInvoker)}的子类");
            }
            //平铺所有待实现接口方法
            var methods =@interfaces.SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance)).ToArray();
           return  methods.Select(mi =>
            {
                var sqlStatementAttribute = mi.GetCustomAttribute<SqlStatementAttribute>();
                var sql = sqlStatementAttribute.SqlStatement;
                var defaultDbInvoker = Activator.CreateInstance(dbInvokerType,dbClient) as AbstractDbInvoker;
                if (defaultDbInvoker != null)
                {
                    defaultDbInvoker.SqlDescriptor = new SqlDescriptor()
                    {
                        Sql = sql,
                    };
                }
                 
                defaultDbInvoker.SqlStatementAttribute = sqlStatementAttribute;
                defaultDbInvoker.SqlDescriptor.ReturnType = mi.ReturnType;
                defaultDbInvoker.SqlDescriptor.Method = mi;
                MatchCollection matches = Regex.Matches(sql, @"@\w+");
                List<string> sqlParams =  matches.Cast<Match>().Select(p => p.Value).ToList();
                List<ParameterInfo> methodParams = mi.GetParameters().ToList();
                var sqlParameterDescriptors  = GenerateSqlParameterDescriptors(mi,sqlParams, methodParams);
                defaultDbInvoker.SqlDescriptor.ParameterDescriptors =sqlParameterDescriptors;
                return defaultDbInvoker;
            }).ToArray();
        }

     
    }
}