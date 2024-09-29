namespace ZStack.QingTui;

public class FollowersResp : ErrorResp
{
    /// <summary>
    /// 使用者的openid列表
    /// </summary>
    [JsonPropertyName("followers")]
    public IReadOnlyList<string>? Followers { get; set; }

    /// <summary>
    /// 是否还有更多的数据
    /// </summary>
    [JsonPropertyName("hasMore")]
    public bool HasMore { get; set; }

    /// <summary>
    /// 使用者总数量
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}
