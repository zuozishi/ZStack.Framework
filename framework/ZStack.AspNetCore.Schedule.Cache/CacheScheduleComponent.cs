using Microsoft.Extensions.DependencyInjection;
using ZStack.AspNetCore.Components;

namespace ZStack.AspNetCore.Schedule.Cache;

[DependsOn(typeof(CacheComponent))]
public class CacheScheduleComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddZStackCacheSchedule();
    }
}
