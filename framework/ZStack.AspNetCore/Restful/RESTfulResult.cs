namespace ZStack.AspNetCore.Restful;

/// <summary>
/// 全局返回结果
/// </summary>
/// <typeparam name="T"></typeparam>
public class RESTfulResult<T>
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 成功状态
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public T? Result { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 附加数据
    /// </summary>
    public object Extras { get; set; } = new object();
}
