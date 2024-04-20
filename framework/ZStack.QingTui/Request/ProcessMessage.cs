namespace ZStack.QingTui;

/// <summary>
/// 代表消息
/// </summary>
public class ProcessMessage
{
    public ProcessMessage() { }

    public ProcessMessage(string title, string body, string url)
    {
        Title = title;
        Body = body;
        Url = url;
    }

    /// <summary>
    /// 标题
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 消息体
    /// </summary>
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// 点击消息后跳转的链接
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}
