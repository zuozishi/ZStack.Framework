using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ZStack.Core;

/// <summary>
/// 应用程序主机构建器
/// </summary>
public static class AppHostBuilder
{
    /// <summary>
    /// 创建主机构建器
    /// </summary>
    /// <param name="args"></param>
    /// <param name="serilogConfiguration"></param>
    /// <param name="consoleLoggerTemplate"></param>
    /// <returns></returns>
    public static HostApplicationBuilder CreateHostBuilder(
        string[]? args = null,
        Action<IConfigurationManager, IHostEnvironment>? configuration = null,
        Action<LoggerConfiguration, IHostEnvironment>? serilogConfiguration = null,
        string? consoleLoggerTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}][{ShortSourceContext}] {Message:lj}{NewLine}{Exception}")
    {
        var builder = Host.CreateApplicationBuilder(args);

        if (configuration != null)
            configuration(builder.Configuration, builder.Environment);
        else
            DefaultConfigureConfiguration(builder.Configuration, builder.Environment);

        Log.Logger = SerilogLogger.CreateConfigurationLogger(builder.Configuration, configure =>
        {
            if (!string.IsNullOrEmpty(consoleLoggerTemplate))
                configure.WriteTo.Console(outputTemplate: consoleLoggerTemplate);
            serilogConfiguration?.Invoke(configure, builder.Environment);
        });
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger);

        builder.Services.AddAutoDependencyInjection();

        return builder;
    }

    /// <summary>
    /// 默认配置选项
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="configuration"></param>
    internal static void DefaultConfigureConfiguration(IConfigurationManager configuration, IHostEnvironment environment)
    {
        var configurationDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            configuration["ConfigurationDirectory"] ?? "Configuration");
        configuration.AddDirectory(configurationDirectory, environment);
        configuration.AddEnvironmentVariables();
        configuration.AddCommandLine(Environment.GetCommandLineArgs());
    }
}
