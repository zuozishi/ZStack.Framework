namespace ZStack.QingTui;

/// <summary>
/// 本地访问密钥持久化器
/// </summary>
public class LocalTokenPersister : ITokenPersister
{
    private string? _token;
    private DateTime _tokenExpireTime = DateTime.Now;
    private string? _jsTicket;
    private DateTime _jsTicketExpireTime = DateTime.Now;

    public string? GetJsTicket(string appId)
    {
        if (DateTime.Now > _jsTicketExpireTime)
            _jsTicket = null;
        return _jsTicket;
    }

    public string? GetToken(string appId)
    {
        if (DateTime.Now > _tokenExpireTime)
            _token = null;
        return _token;
    }

    public void SaveJsTicket(string appId, string ticket, int expire)
    {
        _jsTicket = ticket;
        _jsTicketExpireTime = DateTime.Now.AddSeconds(expire);
    }

    public void SaveToken(string appId, string token, int expire)
    {
        _token = token;
        _tokenExpireTime = DateTime.Now.AddSeconds(expire);
    }
}
