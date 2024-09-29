namespace ZStack.QingTui;

/// <summary>
/// 本地访问密钥持久化器
/// </summary>
public class LocalTokenPersister : ITokenPersister
{
    private string? _token;
    private DateTime _expireTime = DateTime.Now;

    public string? GetToken(string appId)
    {
        if (DateTime.Now > _expireTime)
            _token = null;
        return _token;
    }

    public void SaveToken(string appId, string token, int expire)
    {
        _token = token;
        _expireTime = DateTime.Now.AddSeconds(expire);
    }
}
