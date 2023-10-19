using System.Diagnostics;

namespace ZStack.Core;

/// <summary>
/// 性能追踪器
/// </summary>
public static class PerformanceTracker
{
    /// <summary>
    /// 开始性能追踪单元
    /// </summary>
    /// <param name="callback">结束回调</param>
    /// <returns></returns>
    public static IDisposable CreateScope(Action<TimeSpan> notify)
    {
        return new PerformanceTrackerScope(elapsed =>
        {
            notify(elapsed);
        });
    }
}

/// <summary>
/// 性能追踪单元
/// </summary>
/// <param name="notify"></param>
public class PerformanceTrackerScope: IDisposable
{
    private readonly Action<TimeSpan> _notify;
    private readonly Stopwatch _stopWatch = new();

    public PerformanceTrackerScope(Action<TimeSpan> notify)
    {
        _notify = notify;
        _stopWatch.Start();
    }

    public void Dispose()
    {
        _stopWatch.Stop();
        _notify(TimeSpan.FromTicks(_stopWatch.ElapsedTicks));
        GC.SuppressFinalize(this);
    }
}
