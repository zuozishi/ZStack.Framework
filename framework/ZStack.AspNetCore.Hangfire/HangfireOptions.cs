using Hangfire;
using Hangfire.Console;

namespace ZStack.AspNetCore.Hangfire;

public class HangfireOptions
{
    public BackgroundJobServerOptions Server { get; set; } = new();

    public ConsoleOptions Console { get; set; } = new();
}
