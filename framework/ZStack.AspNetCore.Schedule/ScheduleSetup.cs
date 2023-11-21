using Furion.Schedule;
using ZStack.AspNetCore.Schedule;

namespace Microsoft.Extensions.DependencyInjection;

public static class ScheduleSetup
{
    public static IServiceCollection AddZStackSchedule<TJobClusterRepo, TJobDetailRepo, TJobHistoryRepo>(this IServiceCollection service, Action<ScheduleOptionsBuilder>? configureOptionsBuilder = null)
        where TJobClusterRepo : class, IJobClusterRepo where TJobDetailRepo : class, IJobDetailRepo where TJobHistoryRepo : class, IJobHistoryRepo
    {
        service.AddConfigurableOptions<ScheduleOptions>();
        service.AddSingleton<IJobClusterRepo, TJobClusterRepo>();
        service.AddSingleton<IJobDetailRepo, TJobDetailRepo>();
        service.AddSingleton<IJobHistoryRepo, TJobHistoryRepo>();
        service.AddSchedule(options =>
        {
            options.ClusterId = Environment.MachineName;
            options.AddClusterServer<JobClusterServer>();
            options.AddPersistence<JobPersistence>();
            options.AddMonitor<JobMonitor>();
            configureOptionsBuilder?.Invoke(options);
        });
        return service;
    }
}
