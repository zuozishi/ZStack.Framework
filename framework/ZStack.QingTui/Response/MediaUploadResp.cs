namespace ZStack.QingTui;

/// <summary>
/// 媒体文件上传响应
/// </summary>
public class MediaUploadResp : ErrorResp
{
    /// <summary>
    /// 媒体文件类型
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// 唯一标识
    /// </summary>
    [JsonPropertyName("media_id")]
    public string? MediaId { get; set; }

    /// <summary>
    /// 媒体文件上传时间戳
    /// </summary>
    [JsonPropertyName("created_at")]
    [JsonConverter(typeof(TimeStampMillisecondsConverter))]
    public DateTime CreatedAt { get; set; }
}
