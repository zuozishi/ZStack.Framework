using Microsoft.AspNetCore.Mvc;
using ZStack.AspNetCore.Exceptions;

namespace ZStackWebApplication.Controller;

/// <summary>
/// 测试控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    /// <summary>
    /// 测试方法
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    /// <exception cref="AppException"></exception>
    [HttpGet]
    public async Task<object> Get([FromQuery] string? q)
    {
        await Task.Delay(1);
        if (q == "123")
            throw new AppException(400, "测试错误");
        return new {
            A = q,
            B = DateTime.Now,
            C = DateTime.Now.Date,
            D = TestEnum.B,
            E = Oops.Bah("测试")
        };
    }
}

public enum TestEnum
{
    A = 1,
    B = 2,
    C = 3
}
