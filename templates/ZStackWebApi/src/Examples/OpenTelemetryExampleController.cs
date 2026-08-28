using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace ZStackWebApi.Examples;

[ApiController]
[Route("api/examples/telemetry")]
public sealed class OpenTelemetryExampleController : ControllerBase
{
    private static readonly ActivitySource ActivitySource = new("ZStackWebApi.Example");
    private static readonly Meter Meter = new("ZStackWebApi.Example");
    private static readonly Counter<long> RequestCounter =
        Meter.CreateCounter<long>("zstack_template_example_requests");

    [HttpGet]
    public IActionResult Get()
    {
        using var activity = ActivitySource.StartActivity("template.telemetry.example");
        RequestCounter.Add(1);
        activity?.SetTag("example.component", "opentelemetry");

        return Ok(new
        {
            Message = "OpenTelemetry 已记录一次 Metrics 和 Trace。",
            TraceId = Activity.Current?.TraceId.ToString()
        });
    }
}
