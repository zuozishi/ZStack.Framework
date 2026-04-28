# ZStack.AspNetCore.OpenTelemetry

OpenTelemetry 遥测组件，自动集成 ASP.NET Core 的 Metrics 和 Tracing。

## 快速开始

添加包后，`OpenTelemetryComponent` 会自动注册。配置：

```json
{
  "OpenTelemetry": {
    "ServiceName": "MyApp",
    "ServiceNameSpace": "com.example",
    "ServiceVersion": "1.0.0",
    "Metrics": ["MyApp.Metrics"],
    "Traces": ["MyApp.ActivitySource"]
  }
}
```

## 环境变量

框架支持通过环境变量自动配置 OTLP Export：

| 环境变量 | 说明 |
| -------- | ---- |
| `OTEL_SERVICE_NAME` | 服务名称（优先于配置文件） |
| `OTEL_EXPORTER_OTLP_ENDPOINT` | 设置后自动启用 OTLP 导出 |

## 内置探针对

**Metrics：**
- `AspNetCoreInstrumentation` — ASP.NET Core 请求指标
- `HttpClientInstrumentation` — HttpClient 调用指标
- `RuntimeInstrumentation` — .NET 运行时指标

**Tracing：**
- `AspNetCoreInstrumentation`（含异常记录）
- `HttpClientInstrumentation`

## 手动注册

```csharp
var builder = services.AddZStackOpenTelemetry();
// 可继续扩展 builder
```
