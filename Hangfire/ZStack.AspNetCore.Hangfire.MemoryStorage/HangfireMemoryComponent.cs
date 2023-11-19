using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore.Hangfire.MemoryStorage;

public class HangfireMemoryComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddZStackHangireMemoryStorage();
    }
}
