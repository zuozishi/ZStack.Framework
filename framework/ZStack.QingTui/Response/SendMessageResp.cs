namespace ZStack.QingTui;

public class SendMessageResp : ErrorResp
{
    [JsonPropertyName("data")]
    public string? MessageId { get; set; }
}
