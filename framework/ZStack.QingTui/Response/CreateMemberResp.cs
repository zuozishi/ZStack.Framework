namespace ZStack.QingTui;

public class CreateMemberResp : ErrorResp
{
    /// <summary>
    /// 企业内用户Id
    /// </summary>
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
}
