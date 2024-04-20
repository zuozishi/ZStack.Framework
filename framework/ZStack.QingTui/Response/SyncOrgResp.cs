namespace ZStack.QingTui;

public class SyncOrgResp : ErrorResp
{
    /// <summary>
    /// 组织机构列表
    /// </summary>
    [JsonPropertyName("list")]
    public IReadOnlyList<QtOrgInfo>? Items { get; set; }
}
