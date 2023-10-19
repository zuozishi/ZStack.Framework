using Microsoft.Extensions.DependencyInjection;
using NewLife.Caching;
using ZStack.AspNetCore.Options;

namespace ZStack.AspNetCore.Components;

/// <summary>
/// 缓存组件
/// </summary>
public class CacheComponent : IServiceComponent
{
    public void Load(IServiceCollection services, ComponentContext componentContext)
    {
        services.AddConfigurableOptions<CacheOptions>();
        var options = FurionApp.GetOptions<CacheOptions>();
        switch (options.CacheType)
        {
            case CacheTypes.Redis:
                if (options.Redis == null)
                    throw new Exception("Redis配置不能为空");
                Cache.Default = new FullRedis(options.Redis);
                break;
        }
        services.AddSingleton(Cache.Default);
    }
}
