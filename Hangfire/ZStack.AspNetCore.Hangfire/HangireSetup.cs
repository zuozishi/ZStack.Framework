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
            config.HeartbeatInterval = options.Server.HeartbeatInterval;
            config.ServerCheckInterval = options.Server.ServerCheckInterval;
            config.SchedulePollingInterval = options.Server.SchedulePollingInterval;
            config.ServerName = options.Server.ServerName;
            config.WorkerCount = options.Server.WorkerCount;
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
