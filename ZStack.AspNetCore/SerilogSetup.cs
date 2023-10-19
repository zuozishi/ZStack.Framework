using Microsoft.AspNetCore.Builder;
using Serilog;
using ZStack.Core;

namespace ZStack.AspNetCore;

public static class SerilogSetup
{
    public static void AddSerilogSetup(this WebApplicationBuilder builder, Action<LoggerConfiguration>? configure = null)
    {
        Log.Logger = SerilogLogger.CreateConfigurationLogger(builder.Configuration);
        builder.Host.UseSerilog();
    }
}
