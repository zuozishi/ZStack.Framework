namespace ZStack.QingTui;

public class QtLoginUserInfo
{
    /// <summary>
    /// 用户关注某个轻应用/订阅号后产生的唯一Id
    /// </summary>
    [JsonPropertyName("openid")]
    public string OpenId { get; set; } = string.Empty;

    /// <summary>
    /// 登录者名字
    /// </summary>
    [JsonPropertyName("username")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 对于已认证轻应用/订阅号，同unionid；对于企业内部轻应用/订阅号，同userid
    /// </summary>
    [JsonPropertyName("uid")]
    public string Uid { get; set; } = string.Empty;

    /// <summary>
    /// 同一个轻应用/订阅号在不同企业中的相同用户，拥有相同的unionid
    /// </summary>
    [JsonPropertyName("unionid")]
    public string UnionId { get; set; } = string.Empty;

    /// <summary>
    /// 登录者的头像
    /// </summary>
    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 已认证轻应用在非公共轻应用模式下时，返回当前用户所在的企业id
    /// </summary>
    [JsonPropertyName("domainid")]
    public string DomainId { get; set; } = string.Empty;

    /// <summary>
    /// 用户唯一id
    /// </summary>
    [JsonPropertyName("userid")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 登录者角色，普通用户为0，轻应用管理员为1
    /// </summary>
    [JsonPropertyName("role")]
    public int Role { get; set; }

    /// <summary>
    /// 是否为企业管理员
    /// </summary>
    [JsonPropertyName("is_domain_manager")]
    public bool IsDomainManager { get; set; }

    /// <summary>
    /// 是否为轻应用/订阅号管理员
    /// </summary>
    [JsonPropertyName("is_app_manager")]
    public bool IsAppManager { get; set; }
}
