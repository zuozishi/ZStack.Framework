using Furion.Schedule;
using ZStack.AspNetCore.Schedule.Cache;

namespace Microsoft.Extensions.DependencyInjection;

public static class CacheScheduleSetup
{
    public static IServiceCollection AddZStackCacheSchedule(this IServiceCollection service, Action<ScheduleOptionsBuilder>? configureOptionsBuilder = null)
    {
        service.AddZStackSchedule<CacheJobClusterRepo, CacheJobDetailRepo, CacheJobHistoryRepo>(configureOptionsBuilder);
        return service;
    }
}
