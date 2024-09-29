using Microsoft.AspNetCore.Builder;

namespace ZStack.AspNetCore.Components;

/// <summary>
/// API控制器组件
/// </summary>
public class ApiControllerComponent : IServiceComponent, IApplicationComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddControllers();
    }

    public void Load(IApplicationBuilder app, ComponentContext componentContext)
    {
        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllers();
        });
    }
}
