using Furion.Schedule;
using Microsoft.Extensions.Logging;
using System.Text;

namespace ZStack.AspNetCore.Schedule;

public class JobLogger(ILogger _logger, JobExecutingContext context) : ILogger, IDisposable
{
    private readonly StringBuilder _sb = new();

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _logger.BeginScope(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string level = logLevel switch
        {
            LogLevel.Information => "INF",
            LogLevel.Warning => "WAR",
            LogLevel.Error => "ERR",
            LogLevel.Critical => "CRI",
            _ => "NON",
        };
        _sb.AppendLine($"[{DateTime.Now:HH:mm:ss}][{level}] {formatter(state, exception)}");
        _logger.Log(logLevel, eventId, state, exception, formatter);
    }

    public void Dispose()
    {
        context.Result = _sb.ToString();
        _sb.Clear();
        GC.SuppressFinalize(this);
    }
}
