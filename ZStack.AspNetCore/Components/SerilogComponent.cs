using Microsoft.AspNetCore.Builder;
using Serilog;
using ZStack.Core;

namespace ZStack.AspNetCore.Components;

/// <summary>
/// Serilog日志组件
/// </summary>
public class SerilogComponent : IWebComponent
{
    public void Load(WebApplicationBuilder builder, ComponentContext componentContext)
    {
        Log.Logger = SerilogLogger.CreateConfigurationLogger(builder.Configuration);
        builder.Host.UseSerilog();
    }
}
