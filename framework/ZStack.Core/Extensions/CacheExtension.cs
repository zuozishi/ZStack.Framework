using System.Text.RegularExpressions;

namespace NewLife.Caching;

public static class CacheExtension
{
    /// <summary>
    /// 模糊搜索Key，支持?和*
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static IEnumerable<string> SearchEx(this ICache cache, string pattern)
    {
        IEnumerable<string> keys;
        if (cache is MemoryCache memoryCache)
        {
            keys = memoryCache.Keys;
            if (!string.IsNullOrEmpty(pattern))
            {
                var reg = pattern.Replace("?", ".").Replace("*", ".*");
                keys = keys.Where(k => Regex.IsMatch(k, reg));
            }
            return keys;
        }
        else if (cache is FullRedis redis)
        {
            keys = redis.Search(pattern, 0, int.MaxValue);
            if (!string.IsNullOrEmpty(redis.Prefix))
            {
                var prefix = redis.Prefix;
                keys = keys.Select(k => k.StartsWith(prefix) ? k[prefix.Length..] : k);
            }
            return keys;
        }
        else throw new NotSupportedException($"不支持的缓存类型 {cache.GetType().Name}");
    }
}
