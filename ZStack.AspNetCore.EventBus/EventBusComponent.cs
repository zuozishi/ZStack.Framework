using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore.EventBus;

public class EventBusComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddZStackEventBus();
    }
}
