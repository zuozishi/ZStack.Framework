namespace Microsoft.Extensions.Logging;

/// <summary>
/// 日志扩展
/// </summary>
public static class LoogerExtensions
{
    /// <summary>
    /// 创建一个性能分析的作用域
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IDisposable CreateProflierScope(this ILogger logger, string message, params object?[] args)
        => CreateProflierScope(logger, LogLevel.Information, message, args);

    /// <summary>
    /// 创建一个性能分析的作用域
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="logLevel"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IDisposable CreateProflierScope(this ILogger logger, LogLevel logLevel, string message, params object?[] args)
    {
        logger.LogInformation("[{Action}]" + message, ["START", .. args]);
        var scope = PerformanceTracker.CreateScope(elapsed =>
        {
            logger.Log(logLevel, "[{Action}]" + message + " [{Time}]", ["END", .. args, elapsed]);
        });
        return scope;
    }
}
