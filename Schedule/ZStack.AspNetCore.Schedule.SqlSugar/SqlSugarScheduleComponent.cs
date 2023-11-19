using Microsoft.Extensions.DependencyInjection;
using ZStack.AspNetCore.Components;
using ZStack.AspNetCore.SqlSugar;

namespace ZStack.AspNetCore.Schedule.SqlSugar;

[DependsOn(typeof(CacheComponent), typeof(SqlSugarComponent))]
public class SqlSugarScheduleComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddZStackSqlSugarSchedule();
    }
}
