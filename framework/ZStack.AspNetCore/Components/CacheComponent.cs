using NewLife.Caching;

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
