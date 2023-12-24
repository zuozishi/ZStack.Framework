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
    public static IEnumerable<string> Search(this ICache cache, string pattern)
    {
        var options = App.GetOptions<CacheOptions>();
        IEnumerable<string> keys;
        if (options.CacheType == CacheTypes.Redis)
        {
            var redis = cache as FullRedis
                ?? throw new NotSupportedException();
            redis.Db = options.Redis?.Db ?? 0;
            if (!string.IsNullOrEmpty(options.Redis?.Prefix))
                pattern = options.Redis?.Prefix + pattern;
            keys = redis.Execute(rds => rds.Execute<string[]>("KEYS", pattern)) ?? [];
        }
        else
        {
            keys = (cache.Keys ?? []);
            if (!string.IsNullOrEmpty(pattern))
            {
                var reg = pattern.Replace("?", ".").Replace("*", ".*");
                keys = keys.Where(k => Regex.IsMatch(k, reg));
            }
        }
        return keys;
    }
}
