using Microsoft.Extensions.DependencyInjection;
using ZStack.AspNetCore.Components;

namespace ZStack.AspNetCore.SqlSugar;

[DependsOn(typeof(CacheComponent))]
public class SqlSugarComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddZStackSqlSugar();
    }
}
