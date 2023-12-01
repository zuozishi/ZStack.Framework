using NewLife.Caching;

namespace ZStack.AspNetCore.SqlSugar;

/// <summary>
/// SqlSugar二级缓存 NewLife.Caching 实现
/// </summary>
public class SqlSugarCache : ICacheService
{
    private readonly ICache _cache = Cache.Default;

    public void Add<V>(string key, V value)
        => _cache.Add(key, value);

    public void Add<V>(string key, V value, int cacheDurationInSeconds)
        => _cache.Add(key, value, cacheDurationInSeconds);

    public bool ContainsKey<V>(string key)
        => _cache.ContainsKey(key);

    public V Get<V>(string key)
        => Get<V>(key);

    public IEnumerable<string> GetAllKey<V>()
        => _cache.Search(SqlSugarConst.SqlSugar + "*");

    public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        => _cache.GetOrAdd(cacheKey, (key) => create.Invoke(), cacheDurationInSeconds)!;

    public void Remove<V>(string key)
        => _cache.Remove(key);
}
