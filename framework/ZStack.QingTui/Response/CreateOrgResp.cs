namespace ZStack.QingTui;

public class CreateOrgResp : ErrorResp
{
    /// <summary>
    /// 组织机构Id
    /// </summary>
    [JsonPropertyName("org_id")]
    public string? OrgId { get; set; }
}
