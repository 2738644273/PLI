using System.Reflection;
using PLI.DBManipulation;
using SqlSugar;

namespace OpenCVDemo.SqlSugarImpl;

public class SqlSugarInvoker : AbstractDbInvoker
{
    private readonly ISqlSugarClient _sqlSugarClient;

    public SqlSugarInvoker(object sqlclient) : base(sqlclient)
    {
        _sqlSugarClient = sqlclient as ISqlSugarClient;
    }

    public override object Query(object[] arguments)
    { 
        //参数构造
        var sqlparams = new  List<SugarParameter>();
        for (int i = 0; i < this.SqlDescriptor.ParameterDescriptors.Count; i++)
        {
            var paramDes =  this.SqlDescriptor.ParameterDescriptors[i];
            var par =    new SugarParameter(paramDes.Name, arguments[paramDes.ParameterIndex]);
            sqlparams.Add(par);
        }
        var   sqlparamsArr = sqlparams.ToArray();
        
        Type type = this.SqlDescriptor.ReturnType;

        // 获取 SqlQuery<T> 泛型方法的 MethodInfo
        MethodInfo sqlQueryMethod = typeof(IAdo).GetMethods()
            .Where(m => m.Name == "SqlQuery" && m.IsGenericMethod && m.GetParameters().Length == 2)
            .FirstOrDefault(m => m.GetGenericArguments().Length == 1);
        MethodInfo genericSqlQueryMethod = sqlQueryMethod.MakeGenericMethod(type);
       
        object result = genericSqlQueryMethod.Invoke(_sqlSugarClient.Ado, new object [] { this.SqlDescriptor.Sql, sqlparamsArr });
        return result;
    }

    public override object QueryAsync(object[] arguments)
    {
        //参数构造
        var sqlparams = new  List<SugarParameter>();
        for (int i = 0; i < this.SqlDescriptor.ParameterDescriptors.Count; i++)
        {
            var paramDes =  this.SqlDescriptor.ParameterDescriptors[i];
            var par =    new SugarParameter(paramDes.Name, arguments[paramDes.ParameterIndex]);
            sqlparams.Add(par);
        }
        var   sqlparamsArr = sqlparams.ToArray();
 
        Type type = this.SqlDescriptor.ReturnType;
        var genericType = type.GenericTypeArguments.First();
        if (genericType.IsGenericType)
        {
            genericType =  genericType.GenericTypeArguments.First();
        }

        bool isList = genericType.IsSubclassOf(typeof(ICollection<>));
        // 获取 SqlQuery<T> 泛型方法的 MethodInfo
        MethodInfo sqlQueryMethod = typeof(IAdo).GetMethods()
            .Where(m => m.Name == "SqlQueryAsync" && m.IsGenericMethod && m.GetParameters().Length == 2)
            .FirstOrDefault(m => m.GetGenericArguments().Length == 1);
        MethodInfo genericSqlQueryMethod = sqlQueryMethod.MakeGenericMethod(genericType);

    
            object result =
                genericSqlQueryMethod.Invoke(_sqlSugarClient.Ado, new object[] { this.SqlDescriptor.Sql, sqlparamsArr });
            return result;

    }

    public override object Delete(object[] arguments)
    {
        throw new NotImplementedException();
    }

    public override object DeleteAsync(object[] arguments)
    {
        throw new NotImplementedException();
    }

    public override object Update(object[] arguments)
    {
        throw new NotImplementedException();
    }

    public override object UpdateAsync(object[] arguments)
    {
        throw new NotImplementedException();
    }

    public override object Insert(object[] arguments)
    {
        throw new NotImplementedException();
    }

    public override object InsertAsync(object[] arguments)
    {
        throw new NotImplementedException();
    }
}