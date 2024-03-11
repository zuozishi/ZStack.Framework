using Microsoft.AspNetCore.Builder;

namespace ZStack.AspNetCore.Components;

public class StartupsComponent : IServiceComponent, IApplicationComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddStartups();
    }

    public void Load(IApplicationBuilder app, ComponentContext componentContext)
    {
        app.UseStartups();
    }
}
