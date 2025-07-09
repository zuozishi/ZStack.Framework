using System.Security.Cryptography;
using System.Text;
using ZStack.QingTui.Response;

namespace ZStack.QingTui;

/// <summary>
/// 轻推API客户端
/// </summary>
public partial class QingTuiApiClient
{
    public readonly IFlurlClient RestClient;

    private readonly ILogger? _logger;
    private readonly ITokenPersister _tokenPersister;

    public readonly QingTuiApiClientOptions Options;

    /// <summary>
    /// 应用Id
    /// </summary>
    public string AppId => Options.AppId;

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName => Options.DisplayName;

    public QingTuiApiClient(QingTuiApiClientOptions options)
    {
        Options = options;
        _logger = Options.Logger;
        _tokenPersister = Options.TokenPersister ?? new LocalTokenPersister();
        RestClient = new FlurlClient(Options.Host);
        RestClient.AllowAnyHttpStatus();
        RestClient.BeforeCall(BeforeCall);
    }

    /// <summary>
    /// 刷新Token
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var res = await RestClient.Request("/v1/token")
                .SetQueryParam("grant_type", "client_credential")
                .SetQueryParam("appid", AppId)
                .SetQueryParam("secret", Options.Secret)
                .GetJsonAsync<TokenRefreshResp>(cancellationToken: cancellationToken);
            if (res.ErrorCode != null)
            {
                _logger?.LogError("Token更新失败, appId={AppId}, errCode={ErrorCode}, errMsg={ErrMsg}", AppId, res.ErrorCode, res.ErrMsg);
                return;
            }
            if (string.IsNullOrEmpty(res.AccessToken))
            {
                _logger?.LogError("Token更新失败, appId={AppId}, access_token响应为空", AppId);
                return;
            }
            _tokenPersister.SaveToken(AppId, res.AccessToken, res.ExpiresIn ?? 3600);
            _logger?.LogInformation("刷新Token, token={Token}", res.AccessToken);
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Token更新失败, appId={AppId}", AppId);
        }
    }

    /// <summary>
    /// 获取jsapi_ticket
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> GetJsApiTicketAsync(CancellationToken cancellationToken = default)
    {
        string? cachedTicket = _tokenPersister.GetJsTicket(AppId);
        if (!string.IsNullOrEmpty(cachedTicket))
            return cachedTicket;
        var res = await RestClient.Request("/js/ticket/get")
            .GetJsonAsync<JsApiTicketResp>(cancellationToken: cancellationToken);
        if (res.ErrorCode != null)
        {
            _logger?.LogError("获取JsApiTicket失败, appId={AppId}, errCode={ErrorCode}, errMsg={ErrMsg}", AppId, res.ErrorCode, res.ErrMsg);
            throw Oops.Oh($"获取JsApiTicket失败, errCode={res.ErrorCode}, errMsg={res.ErrMsg}");
        }
        if (string.IsNullOrEmpty(res.Ticket))
        {
            _logger?.LogError("获取JsApiTicket失败, appId={AppId}, ticket响应为空", AppId);
            throw Oops.Oh("获取JsApiTicket失败, ticket响应为空");
        }
        _logger?.LogInformation("获取JsApiTicket成功, ticket={Ticket}", res.Ticket);
        _tokenPersister.SaveJsTicket(AppId, res.Ticket, res.ExpiresIn);
        return res.Ticket;
    }

    /// <summary>
    /// 生成JS-SDK签名信息
    /// </summary>
    /// <param name="originUrl"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<JsSignatureResult> GetJsSignatureAsync(string originUrl, CancellationToken cancellationToken = default)
    {
        // 时间戳服务器端本地生成
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        // 随机字符串，可以自行随机生成
        string nonceStr = Guid.NewGuid().ToString();
        // 保留#号以前的内容
        string url = originUrl.Split('#')[0];
        // 调用轻推JS-SDK接口的临时票据
        string jsapi_ticket = await GetJsApiTicketAsync(cancellationToken);
        // 拼接字符串
        string temp = $"jsapi_ticket={jsapi_ticket}&noncestr={nonceStr}&timestamp={timestamp}&url={url}";
        // 由SHA1工具方法生成本地签名
        string signature = GetSha1(temp);
        return new JsSignatureResult
        {
            AppId = AppId,
            Timestamp = timestamp,
            NonceStr = nonceStr,
            Signature = signature
        };
    }

    /// <summary>
    /// 请求前处理
    /// </summary>
    /// <param name="call"></param>
    /// <returns></returns>
    private async Task BeforeCall(FlurlCall call)
    {
        if (call.Request.Url.Path == "/v1/token")
            return;
        string? token = _tokenPersister.GetToken(AppId);
        if (!QingTuiUtils.VerifyToken(token, AppId))
        {
            await RefreshTokenAsync();
            token = _tokenPersister.GetToken(AppId);
        }
        call.Request.Url.SetQueryParam("access_token", token);
    }

    private static string GetSha1(string input)
    {
        byte[] hashBytes = SHA1.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(hashBytes);
    }
}
