using Furion.Schedule;
using Mapster;
using Microsoft.Extensions.Logging;

namespace ZStack.AspNetCore.Schedule;

/// <summary>
/// 作业处理程序监视器
/// </summary>
/// <param name="_logger"></param>
/// <param name="_historyRepo"></param>
public class JobMonitor(ILogger<JobMonitor> _logger, IJobHistoryRepo _historyRepo) : IJobMonitor
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    public Task OnExecutingAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        _logger.LogInformation("开始执行定时任务, jobId={JobId}, description={Description}, trigger={TriggerId}",
            context.JobId, context.JobDetail.Description, context.TriggerId);
        return Task.CompletedTask;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    public async Task OnExecutedAsync(JobExecutedContext context, CancellationToken stoppingToken)
    {
        var elapsed = context.ExecutedTime - context.OccurrenceTime;
        var seconds = Math.Round(elapsed.TotalSeconds, 2);
        if (context.Exception != null)
        {
            _logger.LogError(context.Exception, "定时任务执行出错, jobId={JobId}, description={Description}, trigger={TriggerId} [{Elapsed}s]",
                context.JobId, context.JobDetail.Description, context.TriggerId, seconds);
        }
        else
        {
            _logger.LogInformation("定时任务执行结束, jobId={JobId}, description={Description}, trigger={TriggerId} [{Elapsed}s]",
                context.JobId, context.JobDetail.Description, context.TriggerId, seconds);
        }
        var jobHistory = context.Adapt<JobHistory>();
        var ex = context.Exception?.InnerException ?? context.Exception;
        jobHistory.ExceptionType = ex?.GetType().FullName;
        jobHistory.ExceptionMessage = ex?.Message;
        jobHistory.ExceptionStackTrace = ex?.StackTrace;
        await _historyRepo.SaveAsync(jobHistory);
    }
}
