using Microsoft.Extensions.Options;
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
        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<CacheOptions>>().Value;
            switch (options.CacheType)
            {
                case CacheTypes.Redis:
                    if (options.Redis == null)
                        throw Oops.Bah("Redis配置不能为空");
                    Cache.Default = new FullRedis(options.Redis);
                    break;
            }
            return Cache.Default;
        });
    }
}
