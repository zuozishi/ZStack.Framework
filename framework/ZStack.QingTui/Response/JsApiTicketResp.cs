namespace ZStack.QingTui.Response;

public class JsApiTicketResp : ErrorResp
{
    /// <summary>
    /// jsapi调用时签名所需的ticket
    /// </summary>
    [JsonPropertyName("ticket")]
    public string Ticket { get; set; } = string.Empty;

    /// <summary>
    /// ticket有效期，单位为秒
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}
