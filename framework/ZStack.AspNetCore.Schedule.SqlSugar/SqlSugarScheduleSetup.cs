using Furion.Schedule;
using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore.Schedule.SqlSugar;

public static class SqlSugarScheduleSetup
{
    public static IServiceCollection AddZStackSqlSugarSchedule(this IServiceCollection service, Action<ScheduleOptionsBuilder>? configureOptionsBuilder = null)
    {
        service.AddZStackSchedule<SqlSugarJobClusterRepo, SqlSugarJobDetailRepo, SqlSugarJobHistoryRepo>(configureOptionsBuilder);
        return service;
    }
}
