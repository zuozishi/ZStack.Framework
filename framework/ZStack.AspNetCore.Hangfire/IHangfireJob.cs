namespace ZStack.AspNetCore.Hangfire;

public interface IHangfireJob : IDisposable, IAsyncDisposable
{
    string JobId { get; }

    string Cron { get; }

    TimeZoneInfo TimeZone { get; }

    Task RunAsync();
}
