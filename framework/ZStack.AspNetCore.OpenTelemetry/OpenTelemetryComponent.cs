using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore.OpenTelemetry;

/// <summary>
/// OpenTelemetry遥测组件
/// </summary>
public class OpenTelemetryComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddZStackOpenTelemetry();
    }
}
