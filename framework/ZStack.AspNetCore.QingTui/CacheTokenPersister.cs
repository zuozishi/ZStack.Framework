using NewLife.Caching;
using ZStack.QingTui;

namespace ZStack.AspNetCore.QingTui;

/// <summary>
/// 缓存令牌持久化器
/// </summary>
public class CacheTokenPersister : ITokenPersister
{
    private readonly ICache _cache = Cache.Default;

    public string? GetToken(string appId)
        => _cache.Get<string?>($"QingTui:Token:{appId}");

    public void SaveToken(string appId, string token, int expire)
    {
        if (expire <= 0)
            expire = 3600;
        _cache.Set($"QingTui:Token:{appId}", token, expire);
    }
}
