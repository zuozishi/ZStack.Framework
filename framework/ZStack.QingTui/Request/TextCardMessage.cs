namespace ZStack.QingTui;

/// <summary>
/// 文本卡片消息
/// </summary>
public class TextCardMessage
{
    public TextCardMessage() { }

    public TextCardMessage(string title, List<Content> contentList, string url, string buttonText)
    {
        Title = title;
        ContentList = contentList;
        Url = url;
        ButtonText = buttonText;
    }

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
    /// 按钮文本，默认为详情，最多6个字符
    /// </summary>
    [JsonPropertyName("button_text")]
    public string? ButtonText { get; set; }

    /// <summary>
    /// 内容列表
    /// </summary>
    [JsonPropertyName("content_list")]
    public List<Content> ContentList { get; set; } = [];

    public class Content
    {
        public Content() { }

        public Content(string text, TextColors color = TextColors.BLACK)
        {
            Text = text;
            Attr = new(color);
        }

        /// <summary>
        /// 内容，最多200个字符
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("attr")]
        public ContentAttr Attr { get; set; } = new();

        public class ContentAttr
        {
            public ContentAttr() { }

            public ContentAttr(TextColors color)
            {
                Color = color;
            }

            /// <summary>
            /// 内容的颜色值，默认黑色
            /// </summary>
            [JsonPropertyName("color")]
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public TextColors Color { get; set; } = TextColors.BLACK;
        }
    }
}
