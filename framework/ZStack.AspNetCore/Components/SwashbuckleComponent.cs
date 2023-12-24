using Microsoft.AspNetCore.Builder;

namespace ZStack.AspNetCore.Components;

public class SwashbuckleComponent : IServiceComponent, IApplicationComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        if (services is null)
            return;
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Load(IApplicationBuilder app, ComponentContext componentContext)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
