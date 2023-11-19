using Furion.Schedule;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ZStack.AspNetCore.Schedule;

public abstract class JobBase<T> : IJob where T : class
{
    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        var options = App.GetOptions<ScheduleOptions>();
        var logger = context.ServiceProvider.GetRequiredService<ILogger<T>>();
        if (options.SaveJobLog)
        {
            using var jobLogger = new JobLogger(logger, context);
            await ExecuteAsync(jobLogger, context, stoppingToken);
        }
        else
        {
            await ExecuteAsync(logger, context, stoppingToken);
        }
    }

    public abstract Task ExecuteAsync(ILogger logger, JobExecutingContext context, CancellationToken stoppingToken);
}
