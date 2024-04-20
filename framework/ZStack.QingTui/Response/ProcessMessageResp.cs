namespace ZStack.QingTui;

public class ProcessMessageResp : ErrorResp
{
    /// <summary>
    /// 响应
    /// </summary>
    [JsonPropertyName("data")]
    public DataModel? Data { get; set; }

    public class DataModel
    {
        [JsonPropertyName("msg_id")]
        public string MessageId { get; set; } = string.Empty;
    }
}

public class ProcessMessageMassResp : ErrorResp
{
    /// <summary>
    /// 响应
    /// </summary>
    [JsonPropertyName("data")]
    public List<ProcessMessageId>? Data { get; set; }
}

public class ProcessMessageId
{
    [JsonPropertyName("open_id")]
    public string OpenId { get; set; } = string.Empty;

    [JsonPropertyName("msg_id")]
    public string MessageId { get; set; } = string.Empty;
}
