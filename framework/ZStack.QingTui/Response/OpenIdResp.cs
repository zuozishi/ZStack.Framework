namespace ZStack.QingTui;

public class OpenIdResp : ErrorResp
{
    [JsonPropertyName("open_id")]
    public string? OpenId { get; set; }
}
