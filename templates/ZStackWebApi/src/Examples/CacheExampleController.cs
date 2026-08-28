using NewLife.Caching;

namespace ZStackWebApi.Examples;

/// <summary>
/// CacheComponent 示例：写入、读取和查看 TTL。
/// </summary>
[ApiController]
[Route("api/examples/cache")]
public sealed class CacheExampleController(ICache cache) : ControllerBase
{
    private const string Key = "zstack:template:cache";

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new CacheExampleResponse(
            cache.Get<string>(Key),
            cache.ContainsKey(Key),
            cache.GetExpire(Key)));
    }

    [HttpPost]
    public IActionResult Set([FromBody] CacheExampleRequest request)
    {
        var value = string.IsNullOrWhiteSpace(request.Value)
            ? "hello from zstack cache"
            : request.Value;
        cache.Set(Key, value, 300);
        return Get();
    }
}

public sealed record CacheExampleRequest(string? Value);

public sealed record CacheExampleResponse(string? Value, bool Exists, TimeSpan Expire);
