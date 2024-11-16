namespace ZStack.AspNetCore.OpenTelemetry;

/// <summary>
/// OpenTelemetry配置类
/// </summary>
public class OpenTelemetryOptions
{
    /// <summary>
    /// 应用命名空间
    /// </summary>
    public string? ServiceNameSpace { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string? ServiceName { get; set; }

    /// <summary>
    /// 应用版本
    /// </summary>
    public string? ServiceVersion { get; set; }

    /// <summary>
    /// 指标配置
    /// </summary>
    public List<string> Metrics { get; set; } = [];

    /// <summary>
    /// 追踪配置
    /// </summary>
    public List<string> Traces { get; set; } = [];
}
