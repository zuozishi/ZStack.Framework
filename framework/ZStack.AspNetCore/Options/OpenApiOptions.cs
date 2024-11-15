namespace ZStack.AspNetCore.Options;

/// <summary>
/// OpenAPI配置类
/// </summary>
public class OpenApiOptions
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; set; } = true;

    /// <summary>
    /// 是否启用Scalar
    /// </summary>
    public bool EnableScalar { get; set; } = true;

    /// <summary>
    /// API组
    /// </summary>
    public Dictionary<string, string> Groups { get; set; } = [];
}
