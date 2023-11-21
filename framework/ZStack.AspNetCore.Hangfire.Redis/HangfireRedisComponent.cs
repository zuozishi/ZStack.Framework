using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore.Hangfire.Redis;

public class HangfireRedisComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddZStackHangireRedisStorage();
    }
}
