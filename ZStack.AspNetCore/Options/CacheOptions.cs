using Furion.ConfigurableOptions;
using NewLife.Caching;

namespace ZStack.AspNetCore.Options;

/// <summary>
/// 缓存配置类
/// </summary>
public class CacheOptions : IConfigurableOptions
{
    /// <summary>
    /// 缓存类型
    /// </summary>
    public CacheTypes CacheType { get; set; } = CacheTypes.Memory;

    /// <summary>
    /// Redis缓存配置
    /// </summary>
    public RedisOptions? Redis { get; set; }
}

/// <summary>
/// 缓存类型
/// </summary>
public enum CacheTypes
{
    Memory,
    Redis
}
