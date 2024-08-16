using OpenCVDemo;
using OpenCVDemo.SqlSugarImpl;
using PLI;
using PLI.Attributes;
using PLI.DBManipulation.Default;
using PLI.Providers.Default;
using SqlSugar;

SqlSugarClient   _sqlSugarClient = new SqlSugarClient(new ConnectionConfig()
    {
        DbType = SqlSugar.DbType.Sqlite,
        ConnectionString ="DataSource="+Path.Combine(Environment.CurrentDirectory, "sqlite-db.db"),
        IsAutoCloseConnection = true,
    },
    db =>
    {
        //单例参数配置，所有上下文生效
        db.Aop.OnLogExecuting = (sql, pars) =>
        {
            Console.WriteLine(UtilMethods.GetNativeSql(sql,pars));
        };
    });

//new DefaultDBInterceptor() 和 new DefaultSqlDescriptor() 应该依赖注入获取
var repositoryFactory = new RepositoryFactory(new DefaultDbInterceptor()
    ,new DefaultSqlDescriptor()
    ,typeof(SqlSugarInvoker),
    _sqlSugarClient
    );

var userRepository =  repositoryFactory.CreateRepository<IUserRepository>();
var b =  await userRepository.GetUserById(new Guid("2182ddcd-2bdb-e0a4-372f-3a14392492bb"));

Console.WriteLine(b);
[Repository]
public interface IUserRepository
{
    /// <summary>
    /// 同步查询User
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Query("SELECT * FROM User WHERE Id = @ID")]
    User GetUserById(int id);
    
    /// <summary>
    /// 异步查询User
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Query("SELECT * FROM User where ID = @Id")]
    Task<User> GetUserById(Guid Id);
    
    /// <summary>
    /// 异步删除User
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Delete("Delete From USER WHERE Id = @ID")]
    Task<int> DeleteUserByIdAsync(int id);
}