namespace ZStack.QingTui;

/// <summary>
/// 键值对消息
/// </summary>
public class KeyValueMessage
{
    public KeyValueMessage() { }

    public KeyValueMessage(string title, IEnumerable<KeyValue> content, ColorTitle? subTitle = null, string? url = null, ColorTitle? footer = null, string? buttonText = null)
    {
        Title = title;
        Content = content.ToList();
        SubTitle = subTitle;
        Url = url;
        Footer = footer;
        ButtonText = buttonText;
    }

    /// <summary>
    /// 标题，最多45个字符
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 首行说明
    /// </summary>
    [JsonPropertyName("sub_title")]
    public ColorTitle? SubTitle { get; set; }

    /// <summary>
    /// 点击后的链接去向地址
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// 正文，最长支持6个键值对
    /// </summary>
    [JsonPropertyName("content")]
    public List<KeyValue> Content { get; set; } = [];

    /// <summary>
    /// 末尾说明
    /// </summary>
    [JsonPropertyName("footer")]
    public ColorTitle? Footer { get; set; }

    /// <summary>
    /// 按钮文本，默认为详情，最多6个字符
    /// </summary>
    [JsonPropertyName("button_text")]
    public string? ButtonText { get; set; }

    public class ColorTitle
    {
        /// <summary>
        /// 首行说明内容
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 首行说明颜色
        /// </summary>
        [JsonPropertyName("color")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TextColors? Color { get; set; }

        public ColorTitle() { }

        public ColorTitle(string title, TextColors? color = null)
        {
            Title = title;
            Color = color;
        }
    }

    public class KeyValue
    {
        /// <summary>
        /// 左侧的栏目名称，最长8个字符
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// 栏目对应的内容，最长45个字符
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 栏目内容的颜色值
        /// </summary>
        [JsonPropertyName("valueColor")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TextColors? ValueColor { get; set; }

        public KeyValue() { }

        public KeyValue(string key, string value, TextColors? valueColor = null)
        {
            Key = key;
            Value = value;
            ValueColor = valueColor;
        }
    }
}
