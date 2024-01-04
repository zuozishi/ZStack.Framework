using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Microsoft.AspNetCore.Builder;
using ZStack.AspNetCore;
using ZStack.AspNetCore.Hangfire;

namespace Microsoft.Extensions.DependencyInjection;

public static class HangireSetup
{
    /// <summary>
    /// 注册Hangfire服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddZStackHangire(this IServiceCollection services, HangfireOptions? options = null, Action<IGlobalConfiguration>? configuration = null)
    {
        options ??= new();
        services.AddHangfire(config =>
        {
            config.UseConsole(options.Console);
            configuration?.Invoke(config);
        });
        services.AddHangfireConsoleExtensions();
        services.AddHangfireServer(config =>
        {
            config.WorkerCount = options.Server.WorkerCount;
            config.Queues = options.Server.Queues;
            config.StopTimeout = options.Server.StopTimeout;
            config.ShutdownTimeout = options.Server.ShutdownTimeout;
            config.SchedulePollingInterval = options.Server.SchedulePollingInterval;
            config.HeartbeatInterval = options.Server.HeartbeatInterval;
            config.ServerTimeout = options.Server.ServerTimeout;
            config.ServerCheckInterval = options.Server.ServerCheckInterval;
            config.CancellationCheckInterval = options.Server.CancellationCheckInterval;
            config.FilterProvider = options.Server.FilterProvider;
            config.Activator = options.Server.Activator;
            config.TimeZoneResolver = options.Server.TimeZoneResolver;
            config.TaskScheduler = options.Server.TaskScheduler;
            config.ServerName = options.Server.ServerName;
        });
        return services;
    }

    /// <summary>
    /// 扫描注册定时任务
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder RegisterHangireJobs(this IApplicationBuilder app)
    {
        var jobs = app.ApplicationServices.GetServices<IHangfireJob>();
        foreach (var job in jobs)
        {
            App.Logger.Information("注册定时任务：{JobId} [{Type}][{Cron}]", job.JobId, job.GetType().FullName, job.Cron);
            RecurringJob.AddOrUpdate(job.JobId, () => job.RunAsync(), job.Cron, new RecurringJobOptions
            {
                TimeZone = job.TimeZone
            });
        }
        return app;
    }
}
