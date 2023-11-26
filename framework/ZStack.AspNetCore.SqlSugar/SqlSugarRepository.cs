using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore.SqlSugar;

/// <summary>
/// SqlSugar 实体仓储
/// </summary>
/// <typeparam name="T"></typeparam>
public class SqlSugarRepository<T> : SimpleClient<T> where T : class, new()
{
    public SqlSugarRepository()
    {
        var sqlSugarService = App.GetRequiredService<ISqlSugarService>();

        Context = sqlSugarService.Get();

        // 若实体贴有系统表特性，则返回默认库连接
        if (typeof(T).IsDefined(typeof(SysTableAttribute), false))
            return;

        // 若实体贴有多库特性，则返回指定库连接
        if (typeof(T).IsDefined(typeof(TenantAttribute), false))
        {
            Context = sqlSugarService.Get(typeof(T).GetCustomAttribute<TenantAttribute>()!.configId.ToString());
            return;
        }
    }
}
