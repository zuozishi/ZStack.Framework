using ZStack.SqlSugar;

namespace ZStackWebApi.Examples;

/// <summary>
/// SqlSugarComponent 示例：读取当前数据源配置，不会主动打开数据库连接。
/// </summary>
[ApiController]
[Route("api/examples/sqlsugar")]
public sealed class SqlSugarExampleController(ISqlSugarService sqlSugar) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Configured = sqlSugar.Contains(),
            ConnectionCount = sqlSugar.Options.ConnectionConfigs.Count,
            Hint = "调用 sqlSugar.Get() 前请确认 ConnectionString 和 DbType 已按实际数据库修改。"
        });
    }
}
