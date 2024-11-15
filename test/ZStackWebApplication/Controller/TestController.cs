using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using ZStack.Core.Exceptions;

namespace ZStackWebApplication.Controller;

/// <summary>
/// 测试控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
//[ApiExplorerSettings(GroupName = "v2")]
public class TestController(ILogger<TestController> _logger, ISqlSugarClient _db) : ControllerBase
{
    private static readonly HttpClient HttpClient = new();

    /// <summary>
    /// 测试方法
    /// </summary>
    /// <returns></returns>
    /// <exception cref="AppException"></exception>
    [HttpGet]
    public async Task<int> Get(CancellationToken cancellationToken = default)
    {
        var res = await HttpClient.GetStringAsync("https://git.ctmcc.cn/explore", cancellationToken);
        var total = await _db.Ado.GetIntAsync("SELECT COUNT(1) FROM scraper_gfriends");
        return total;
    }
}
