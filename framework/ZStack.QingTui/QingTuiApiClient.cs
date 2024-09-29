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
}
