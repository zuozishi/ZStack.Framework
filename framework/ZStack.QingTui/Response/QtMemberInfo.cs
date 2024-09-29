namespace ZStack.QingTui;

/// <summary>
/// 企业成员信息
/// </summary>
public class QtMemberInfo
{
    /// <summary>
    /// 名称
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 邮箱
    /// </summary>
    [JsonPropertyName("mail")]
    public string? Mail { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    /// <summary>
    /// 是否访客，yes是；no否
    /// </summary>
    [JsonPropertyName("guest")]
    public string Guest { get; set; } = string.Empty;

    /// <summary>
    /// OpenId
    /// </summary>
    [JsonPropertyName("open_id")]
    public string OpenId { get; set; } = string.Empty;

    /// <summary>
    /// 企业内用户Id
    /// </summary>
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [JsonPropertyName("mobile")]
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 账号Id
    /// </summary>
    [JsonPropertyName("account_id")]
    public string AccountId { get; set; } = string.Empty;

    /// <summary>
    /// 所在的组织机构列表
    /// </summary>
    [JsonPropertyName("org_list")]
    public IReadOnlyList<string> OrgList { get; set; } = [];

    /// <summary>
    /// 所属组织Id
    /// </summary>
    [JsonPropertyName("org_id")]
    public string OrgId { get; set; } = string.Empty;

    /// <summary>
    /// 工号
    /// </summary>
    [JsonPropertyName("employee_id")]
    public string? EmployeeId { get; set; }

    /// <summary>
    /// 状态，0退出企业，1在企业中
    /// </summary>
    [JsonPropertyName("status")]
    public int Status { get; set; }

    /// <summary>
    /// 状态，0退出企业，1在企业中
    /// </summary>
    [JsonPropertyName("更新时间")]
    [JsonConverter(typeof(TimeStampMillisecondsConverter))]
    public DateTime? UpdateTime { get; set; }
}
