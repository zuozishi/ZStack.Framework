namespace NewLife.Caching;

public static class CacheExtension
{
    /// <summary>
    /// 根据前缀获取所有的Key
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public static IEnumerable<string> Keys(this ICache cache, string prefix)
    {
        var options = App.GetOptions<CacheOptions>();
        var redisPrefix = options.Redis?.Prefix;
        if (!string.IsNullOrEmpty(redisPrefix))
            prefix = redisPrefix + prefix;
        return cache.Keys.Where(x => x.StartsWith(prefix));
    }
}
