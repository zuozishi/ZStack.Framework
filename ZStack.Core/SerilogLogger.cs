using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ZStack.Core;

/// <summary>
/// 日志记录器
/// </summary>
public static class SerilogLogger
{
    /// <summary>
    /// 创建控制台应用程序日志记录器
    /// </summary>
    /// <returns></returns>
    public static Logger CreateConsoleAppLogger(Action<LoggerConfiguration>? configure = null,
        string outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}][{ShortSourceContext}] {Message:lj}{NewLine}{Exception}")
    {
        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.With(new ShortSourceContextEnricher())
            .WriteTo.Console(outputTemplate: outputTemplate);
        configure?.Invoke(loggerConfiguration);
        return loggerConfiguration.CreateLogger();
    }

    /// <summary>
    /// 创建通过配置文件配置的日志记录器
    /// </summary>
    /// <returns></returns>
    public static Logger CreateConfigurationLogger(Action<LoggerConfiguration>? configure = null, string filePrefix = "logger")
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{filePrefix}.json")
            .AddJsonFile($"{filePrefix}.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            .Build();
        return CreateConfigurationLogger(configuration, configure);
    }

    /// <summary>
    /// 创建通过配置文件配置的日志记录器
    /// </summary>
    /// <returns></returns>
    public static Logger CreateConfigurationLogger(IConfigurationRoot configuration, Action<LoggerConfiguration>? configure = null)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.With(new ShortSourceContextEnricher())
            .ReadFrom.Configuration(configuration);
        configure?.Invoke(loggerConfiguration);
        return loggerConfiguration.CreateLogger();
    }
}

public class ShortSourceContextEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (!logEvent.Properties.ContainsKey("SourceContext"))
            return;
        var sourceContext = logEvent.Properties["SourceContext"];
        if (sourceContext == null)
            return;
        var value = ((ScalarValue)sourceContext).Value;
        if (value == null)
            return;
        var typeStr = value.ToString();
        if (typeStr == null)
            return;
        var property = propertyFactory.CreateProperty("ShortSourceContext", typeStr.Split('.').Last());
        logEvent.AddPropertyIfAbsent(property);
    }
}
