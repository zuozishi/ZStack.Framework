using NewLife.Caching;
using ZStack.Core.Exceptions;

namespace ZStack.AspNetCore.Components;

/// <summary>
/// 缓存组件
/// </summary>
public class CacheComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddZStackOptions<CacheOptions>();
        var options = App.GetOptions<CacheOptions>();
        if (options.CacheType == CacheTypes.Redis)
        {
            if (options.Redis == null)
                throw Oops.Bah("Redis配置不能为空");
            Cache.Default = new FullRedis(options.Redis);
        }
        services.AddSingleton(Cache.Default);
    }
}
