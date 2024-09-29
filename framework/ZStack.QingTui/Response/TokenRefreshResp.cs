namespace ZStack.QingTui;

/// <summary>
/// 获取Token响应
/// </summary>
public class TokenRefreshResp : ErrorResp
{
    /// <summary>
    /// 凭证有效时间，单位：秒
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; set; }

    /// <summary>
    /// 刷新凭证，有效期30天
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// 接口调用凭证
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
}
