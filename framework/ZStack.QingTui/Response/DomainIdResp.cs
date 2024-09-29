namespace ZStack.QingTui;

public class DomainIdResp : ErrorResp
{
    /// <summary>
    /// 企业Id
    /// </summary>
    [JsonPropertyName("domainId")]
    public string? DomainId { get; set; }
}
