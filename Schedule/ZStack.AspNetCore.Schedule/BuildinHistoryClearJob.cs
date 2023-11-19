using Furion.Schedule;
using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore.Schedule;

/// <summary>
/// 过期作业执行记录清理
/// </summary>
[JobDetail("BuildinHistoryClearJob", true, "过期作业执行记录清理")]
[PeriodHours(3, TriggerId = "Every3Hours", RunOnStart = true)]
public class BuildinHistoryClearJob : IJob
{
    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        var options = App.GetOptions<ScheduleOptions>();
        var historyRepo = context.ServiceProvider.GetRequiredService<IJobHistoryRepo>();
        await historyRepo.ClearAsync(options.MaxHistoryCount, options.MaxHistoryDays);
    }
}
