using Hangfire.Server;
using ZStack.AspNetCore.Hangfire;

namespace ZStackWebApi.Examples;

/// <summary>
/// HangfireComponent 示例：每五分钟输出一次日志。
/// </summary>
[Injection(Pattern = InjectionPatterns.FirstInterface)]
public sealed class HangfireExampleJob : HangfireJobBase<HangfireExampleJob>
{
    public override string JobId => "zstack-template-heartbeat";

    public override string Cron => "*/5 * * * *";

    public override Task RunAsync(PerformingContext? context, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Hangfire 示例任务执行于 {Time}", DateTimeOffset.Now);
        return Task.CompletedTask;
    }
}
