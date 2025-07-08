namespace ZStack.QingTui.Response;

public class JsSignatureResult
{
    /// <summary>
    /// 轻应用/订阅号的唯一标识
    /// </summary>
    public string AppId { get; set; } = string.Empty;

    /// <summary>
    /// 生成签名的时间戳
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// 生成签名的随机串
    /// </summary>
    public string NonceStr { get; set; } = string.Empty;

    /// <summary>
    /// 签名
    /// </summary>
    public string Signature { get; set; } = string.Empty;
}
