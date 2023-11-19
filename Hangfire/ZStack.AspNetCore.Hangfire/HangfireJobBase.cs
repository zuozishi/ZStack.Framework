using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ZStack.AspNetCore.Hangfire;

public abstract class HangfireJobBase<T>(IServiceProvider serviceProvider) : IHangfireJob
{
    public readonly IServiceProvider ServiceProvider = serviceProvider;

    public readonly ILogger<T> Logger = serviceProvider.GetRequiredService<ILogger<T>>();

    public readonly IProgressBarFactory ProgressBarFactory = serviceProvider.GetRequiredService<IProgressBarFactory>();

    public readonly IJobManager JobManager = serviceProvider.GetRequiredService<IJobManager>();

    public readonly PerformingContext? PerformingContext = serviceProvider.GetService<PerformingContext>();

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

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}
