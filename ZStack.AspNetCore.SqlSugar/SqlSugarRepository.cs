using SqlSugar;
using ZStack.Core.Attributes;

namespace ZStack.AspNetCore.SqlSugar;

/// <summary>
/// SqlSugar 实体仓储
/// </summary>
/// <typeparam name="T"></typeparam>
public class SqlSugarRepository<T> : SimpleClient<T> where T : class, new()
{
    protected ITenant? iTenant = null;

    public SqlSugarRepository()
    {
        iTenant = App.GetRequiredService<ISqlSugarClient>().AsTenant();
        Context = iTenant.GetConnectionScope(SqlSugarConst.MainConfigId);

        // 若实体贴有多库特性，则返回指定库连接
        if (typeof(T).IsDefined(typeof(TenantAttribute), false))
        {
            Context = iTenant.GetConnectionScopeWithAttr<T>();
            return;
        }

        // 若实体贴有系统表特性，则返回默认库连接
        if (typeof(T).IsDefined(typeof(SysTableAttribute), false))
        {
            Context = iTenant.GetConnectionScope(SqlSugarConst.MainConfigId);
            return;
        }
    }
}
