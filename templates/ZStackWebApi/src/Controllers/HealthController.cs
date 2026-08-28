namespace ZStackWebApi.Controllers;

/// <summary>
/// 基础健康检查，同时展示当前实际加载的 ZStack 组件。
/// </summary>
[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var components = App.Components
            .Select(component => component.ComponentType.Name)
            .Distinct()
            .Order()
            .ToArray();

        return Ok(new
        {
            Status = "ok",
            Application = App.HostEnvironment?.ApplicationName,
            Environment = App.HostEnvironment?.EnvironmentName,
            Components = components
        });
    }
}
