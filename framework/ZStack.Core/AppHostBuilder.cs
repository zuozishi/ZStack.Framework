using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
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
    public static HostApplicationBuilder CreateHostBuilder(string[]? args = null, Action<LoggerConfiguration>? serilogConfiguration = null, string? consoleLoggerTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}][{ShortSourceContext}] {Message:lj}{NewLine}{Exception}")
    {
        var builder = Host.CreateApplicationBuilder(args);
        ConfigureConfiguration(builder.Environment, builder.Configuration);

        Log.Logger = SerilogLogger.CreateConfigurationLogger(builder.Configuration, configure =>
        {
            if (!string.IsNullOrEmpty(consoleLoggerTemplate))
                configure.WriteTo.Console(outputTemplate: consoleLoggerTemplate);
            serilogConfiguration?.Invoke(configure);
        });
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger);

        builder.Services.AddAutoDependencyInjection();

        return builder;
    }

    /// <summary>
    /// 配置选项
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="configuration"></param>
    internal static void ConfigureConfiguration(IHostEnvironment environment, IConfigurationManager configuration)
    {
        var env = environment.EnvironmentName;
        var configurationDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            configuration["ConfigurationDirectory"] ?? "Configuration");
        var files = new Matcher()
            .AddInclude("*.json")
            .AddInclude("*.ini")
            .AddInclude("*.yml")
            .AddInclude("*.yaml")
            .AddExclude("*.schema.json")
            .GetResultsInFullPath(configurationDirectory);
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            var pt = fileName.Split('.');
            if (pt.Length > 2 && !env.Equals(pt[^2], StringComparison.OrdinalIgnoreCase))
                continue;
            if (fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                configuration.AddJsonFile(file, optional: true, reloadOnChange: true);
            }
            else if (fileName.EndsWith(".ini", StringComparison.OrdinalIgnoreCase))
            {
                configuration.AddIniFile(file, optional: true, reloadOnChange: true);
            }
            else if (fileName.EndsWith(".yml", StringComparison.OrdinalIgnoreCase) ||
                fileName.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase))
            {
                configuration.AddYamlFile(file, optional: true, reloadOnChange: true);
            }
        }
        configuration.AddEnvironmentVariables();
        configuration.AddCommandLine(Environment.GetCommandLineArgs());
    }
}
