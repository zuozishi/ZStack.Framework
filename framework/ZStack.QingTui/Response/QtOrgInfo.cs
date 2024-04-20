namespace ZStack.QingTui;

public class QtOrgInfo
{
    /// <summary>
    /// 组织机构Id
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 组织机构名称
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 父组织机构Id
    /// </summary>
    [JsonPropertyName("parent_id")]
    public string ParentId { get; set; } = string.Empty;

    /// <summary>
    /// 组织机构显示顺序
    /// </summary>
    [JsonPropertyName("sequence")]
    public int Sequence { get; set; }

    /// <summary>
    /// 组织机构等级
    /// </summary>
    [JsonPropertyName("grade")]
    public int Grade { get; set; }

    /// <summary>
    /// 是否存在，0 组织机构已不存在，被删除；1 组织机构还存在，仅更新
    /// </summary>
    [JsonPropertyName("status")]
    public int? Status { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonPropertyName("update_time")]
    [JsonConverter(typeof(TimeStampMillisecondsConverter))]
    public DateTime? UpdateTime { get; set; }
}
