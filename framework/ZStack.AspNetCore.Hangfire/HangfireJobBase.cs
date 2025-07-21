using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZStack.Core.Exceptions;

namespace ZStack.AspNetCore.Hangfire;

public abstract class HangfireJobBase<T> : IHangfireJob
{
    public readonly IServiceProvider ServiceProvider;

    public readonly ILogger<T> Logger;

    public readonly IProgressBarFactory ProgressBarFactory;

    public readonly IJobManager JobManager;

    public readonly PerformingContext? PerformingContext;

    public HangfireJobBase()
    {
        ServiceProvider = App.ServiceProvider
            ?? throw Oops.Oh("无法获取应用程序服务提供者(ServiceProvider)，请确保应用已正确初始化。");
        Logger = ServiceProvider.GetRequiredService<ILogger<T>>();
        ProgressBarFactory = ServiceProvider.GetRequiredService<IProgressBarFactory>();
        JobManager = ServiceProvider.GetRequiredService<IJobManager>();
        PerformingContext = ServiceProvider.GetService<PerformingContext>();
    }

    public abstract string JobId { get; }

    public abstract string Cron { get; }

    public virtual TimeZoneInfo TimeZone { get; } = TimeZoneInfo.Local;

    public abstract Task RunAsync(PerformingContext? context, CancellationToken cancellationToken);

    public Task RunAsync()
    {
        CancellationToken cancellationToken = default;
        try
        {
            var jobCancellationToken = ServiceProvider.GetService<IJobCancellationToken>()?.ShutdownToken;
            if (jobCancellationToken != null)
                cancellationToken = jobCancellationToken.Value;
        }
        catch { }
        return RunAsync(PerformingContext, cancellationToken);
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public virtual ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}
