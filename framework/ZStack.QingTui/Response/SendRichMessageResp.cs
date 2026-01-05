namespace ZStack.QingTui;

public class SendRichMessageResp : ErrorResp
{
    [JsonPropertyName("msg_id")]
    public string? MessageId { get; set; }
}
