namespace ZStack.QingTui;

public partial class QingTuiApiClient
{
    #region 文字消息

    /// <summary>
    /// 群发文字消息
    /// </summary>
    /// <param name="content">发送的文本消息内容</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendTextMessageAsync(string content, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/text/send/service")
            .PostJsonAsync(new
            {
                message = new { content }
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 单发文字消息
    /// </summary>
    /// <param name="content">发送的文本消息内容</param>
    /// <param name="openId">要发送的用户openid</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendTextMessageAsync(string content, string openId, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/text/send/single")
            .PostJsonAsync(new
            {
                to_user = openId,
                message = new { content }
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 给部分人发文字消息
    /// </summary>
    /// <param name="content">发送的文本消息内容</param>
    /// <param name="openIds">要发送的用户openid列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendTextMessageAsync(string content, IEnumerable<string> openIds, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/text/send/mass")
            .PostJsonAsync(new
            {
                to_users = openIds,
                message = new { content }
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    #endregion

    #region 图片消息

    /// <summary>
    /// 群发图片消息
    /// </summary>
    /// <param name="mediaId">图片id，通过上传多媒体文件方法获得</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendImageMessageAsync(string mediaId, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/image/send/service")
            .PostJsonAsync(new
            {
                message = new
                {
                    media_id = mediaId
                }
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 单发图片消息
    /// </summary>
    /// <param name="mediaId">图片id，通过上传多媒体文件方法获得</param>
    /// <param name="openId">要发送的用户openid</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendImageMessageAsync(string mediaId, string openId, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/image/send/single")
            .PostJsonAsync(new
            {
                to_user = openId,
                message = new
                {
                    media_id = mediaId
                }
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 给部分人发图片消息
    /// </summary>
    /// <param name="mediaId">图片id，通过上传多媒体文件方法获得</param>
    /// <param name="openIds">要发送的用户openid列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendImageMessageAsync(string mediaId, IEnumerable<string> openIds, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/image/send/mass")
            .PostJsonAsync(new
            {
                to_users = openIds,
                message = new
                {
                    media_id = mediaId
                }
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    #endregion

    #region 文本卡片消息

    /// <summary>
    /// 单发文本卡片消息
    /// </summary>
    /// <param name="message">文本卡片消息</param>
    /// <param name="openId">要发送的用户openid</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendTextCardMessageAsync(TextCardMessage message, string openId, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/textCard/send/single")
            .PostJsonAsync(new
            {
                to_user = openId,
                message
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 给部分人发文本卡片消息
    /// </summary>
    /// <param name="message">文本卡片消息</param>
    /// <param name="openIds">要发送的用户openid列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendTextCardMessageAsync(TextCardMessage message, IEnumerable<string> openIds, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/textCard/send/mass")
            .PostJsonAsync(new
            {
                to_users = openIds,
                message
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    #endregion

    #region 图文消息

    /// <summary>
    /// 群发图文消息
    /// </summary>
    /// <param name="articles">文章列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendNewsMessageAsync(List<NewsMessage> articles, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/news/send/service")
        .PostJsonAsync(new
        {
            message = new
            {
                article_list = articles
            }
        }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 单发图文消息
    /// </summary>
    /// <param name="articles">文章列表</param>
    /// <param name="openId">要发送的用户openid</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendNewsMessageAsync(List<NewsMessage> articles, string openId, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/news/send/single")
        .PostJsonAsync(new
        {
            to_user = openId,
            message = new
            {
                article_list = articles
            }
        }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 给部分人发图文消息
    /// </summary>
    /// <param name="articles">文章列表</param>
    /// <param name="openIds">要发送的用户openid列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendNewsMessageAsync(List<NewsMessage> articles, IEnumerable<string> openIds, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/news/send/mass")
        .PostJsonAsync(new
        {
            to_users = openIds,
            message = new
            {
                article_list = articles
            }
        }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    #endregion

    #region 键值对消息

    /// <summary>
    /// 单发键值对消息
    /// </summary>
    /// <param name="message">键值对消息</param>
    /// <param name="openId">要发送的用户openid</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendKeyValueMessageAsync(KeyValueMessage message, string openId, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/keyValue/send/single")
            .PostJsonAsync(new
            {
                to_user = openId,
                message
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 给部分人发键值对消息
    /// </summary>
    /// <param name="message">键值对消息</param>
    /// <param name="openIds">要发送的用户openid列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendKeyValueMessageAsync(KeyValueMessage message, IEnumerable<string> openIds, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/keyValue/send/mass")
            .PostJsonAsync(new
            {
                to_users = openIds,
                message
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    #endregion

    #region 文件消息

    /// <summary>
    /// 群发文件消息
    /// </summary>
    /// <param name="mediaId">文件id，通过上传多媒体文件方法获得</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendFileMessageAsync(string mediaId, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/file/send/service")
            .PostJsonAsync(new
            {
                message = new
                {
                    media_id = mediaId
                }
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 单发文件消息
    /// </summary>
    /// <param name="mediaId">文件id，通过上传多媒体文件方法获得</param>
    /// <param name="openId">要发送的用户openid</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendFileMessageAsync(string mediaId, string openId, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/file/send/single")
            .PostJsonAsync(new
            {
                to_user = openId,
                message = new
                {
                    media_id = mediaId
                }
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 给部分人发文件消息
    /// </summary>
    /// <param name="mediaId">文件id，通过上传多媒体文件方法获得</param>
    /// <param name="openIds">要发送的用户openid列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendFileMessageAsync(string mediaId, IEnumerable<string> openIds, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/file/send/mass")
            .PostJsonAsync(new
            {
                to_users = openIds,
                message = new
                {
                    media_id = mediaId
                }
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    #endregion

    #region 待办消息

    /// <summary>
    /// 单发键待办消息
    /// </summary>
    /// <param name="message">待办消息</param>
    /// <param name="openId">要发送的用户openid</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendProcessMessageAsync(ProcessMessage message, string openId, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/process/send/single")
            .PostJsonAsync(new
            {
                to_user = openId,
                message
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<ProcessMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.Data?.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 给部分人发待办消息
    /// </summary>
    /// <param name="message">待办消息</param>
    /// <param name="openIds">要发送的用户openid列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<ProcessMessageId>> SendProcessMessageAsync(ProcessMessage message, IEnumerable<string> openIds, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/process/send/mass")
            .PostJsonAsync(new
            {
                to_users = openIds,
                message
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<ProcessMessageMassResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.Data ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 待办消息置为已处理
    /// </summary>
    /// <param name="messageId">此消息的id，发送待办消息后返回的消息id</param>
    /// <param name="openId">消息对应的用户openid</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task MarkProcessCompleteAsync(string messageId, string openId, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/process/complete")
            .PostJsonAsync(new
            {
                msg_id = messageId,
                open_id = openId
            }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<ErrorResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
    }

    #endregion

    #region 卡片消息

    /// <summary>
    /// 卡片消息序列化配置
    /// </summary>
    private static readonly JsonSerializerOptions _cardMessageSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>
    /// 解析卡片消息
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private static string ParseCardMessage(object content)
    {
        if (content is string str)
            return str;
        return JsonSerializer.Serialize(content, _cardMessageSerializerOptions);
    }

    /// <summary>
    /// 群发卡片消息
    /// </summary>
    /// <param name="content">卡片消息</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendCardMessageAsync(object content, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/card/send/service")
        .PostJsonAsync(new
        {
            message = new
            {
                content = ParseCardMessage(content)
            }
        }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 单发卡片消息
    /// </summary>
    /// <param name="content">卡片消息</param>
    /// <param name="openId">要发送的用户openid</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendCardMessageAsync(object content, string openId, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/card/send/single")
        .PostJsonAsync(new
        {
            to_user = openId,
            message = new
            {
                content = ParseCardMessage(content)
            }
        }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 给部分人发卡片消息
    /// </summary>
    /// <param name="content">卡片消息</param>
    /// <param name="openIds">要发送的用户openid列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendCardMessageAsync(object content, IEnumerable<string> openIds, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/card/send/mass")
        .PostJsonAsync(new
        {
            to_users = openIds,
            message = new
            {
                content = ParseCardMessage(content)
            }
        }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    #endregion

    #region 富文本消息

    /// <summary>
    /// 群发富文本消息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendRichMessageAsync(object message, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/service/send")
        .PostJsonAsync(new
        {
            msgtype = "richmsg",
            richMsg = message
        }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendRichMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    /// <summary>
    /// 给部分人发送富文本消息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="openIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendRichMessageAsync(object message, IEnumerable<string> openIds, CancellationToken cancellationToken = default)
    {
        var resp = await RestClient.Request("/v1/message/mass/send")
        .PostJsonAsync(new
        {
            touser = openIds,
            msgtype = "richmsg",
            richMsg = message
        }, cancellationToken: cancellationToken);
        var res = await resp.GetJsonAsync<SendRichMessageResp>();
        if (res.ErrorCode != null && res.ErrorCode != 0)
            throw Oops.Throw(res.ErrorCode.Value, res.ErrMsg ?? "无响应");
        return res.MessageId ?? throw Oops.Bah("无响应");
    }

    #endregion
}
