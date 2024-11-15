using OpenTelemetry.Exporter;

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

    /// <summary>
    /// OTLP导出器
    /// </summary>
    public List<OtlpEndpoint> OtlpExporters { get; set; } = [];
}

public class OtlpEndpoint
{
    /// <summary>
    /// 导出器名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// OTLP服务地址
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// 协议
    /// </summary>
    public OtlpExportProtocol Protocol { get; set; } = OtlpExportProtocol.Grpc;
}
