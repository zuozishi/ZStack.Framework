using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ZStack.Core;

/// <summary>
/// 依赖注入
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// 创建控制台应用程序依赖注入容器
    /// </summary>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceProvider CreateConsoleAppServiceProvider(
        Action<ServiceCollection> servicesConfigure,
        bool addLogger = true,
        Action<LoggerConfiguration>? loggerConfigure = null)
    {
        var services = new ServiceCollection();
        if (addLogger)
        {
            var logger = SerilogLogger.CreateConsoleAppLogger(loggerConfigure);
            services.AddSingleton<ILogger>(logger);
        }
        servicesConfigure(services);
        return services.BuildServiceProvider();
    }
}
