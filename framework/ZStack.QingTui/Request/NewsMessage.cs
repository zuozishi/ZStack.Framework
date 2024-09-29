namespace ZStack.QingTui;

public class NewsMessage
{
    /// <summary>
    /// 标题，最多45个字符
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 点击后的链接去向地址
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 摘要，最多120个字符
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图片media_id
    /// </summary>
    [JsonPropertyName("thumbMediaId")]
    public string ThumbMediaId { get; set; } = string.Empty;
}
