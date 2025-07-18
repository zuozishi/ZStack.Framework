using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZStack.AspNetCore.UnifyResult;

/// <summary>
/// 统一返回结果
/// </summary>
/// <typeparam name="T"></typeparam>
public class RESTfulResult<T>
{
    [Required]
    [Description("状态码")]
    [DefaultValue(200)]
    public int Code { get; set; }

    [Required]
    [Description("成功状态")]
    public bool Success { get; set; }

    [Description("数据")]
    public T? Result { get; set; }

    [Required]
    [Description("错误信息")]
    [DefaultValue("success")]
    public string Message { get; set; } = string.Empty;

    [Description("附加数据")]
    public object? Extras { get; set; }

    [Required]
    [Description("跟踪Id")]
    [DefaultValue("<跟踪Id>")]
    public string TraceId { get; set; } = string.Empty;
}
