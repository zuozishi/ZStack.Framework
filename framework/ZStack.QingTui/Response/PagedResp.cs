namespace ZStack.QingTui;

public class PagedResp<T> : ErrorResp
{
    /// <summary>
    /// 总数
    /// </summary>
    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }

    /// <summary>
    /// 是否还有更多未读的数据
    /// </summary>
    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    /// <summary>
    /// 数据列表
    /// </summary>
    [JsonPropertyName("result_list")]
    public IReadOnlyList<T>? Items { get; set; }
}
