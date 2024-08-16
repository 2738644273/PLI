namespace PLI.DBManipulation
{
    /// <summary>
    /// 数据库操作拦截器接口
    /// </summary>
    public interface IDbManipulationInterceptor
    {
       
        object ExecuteCommand(AbstractDbInvoker actionInvoker, object[] arguments);
    }
}