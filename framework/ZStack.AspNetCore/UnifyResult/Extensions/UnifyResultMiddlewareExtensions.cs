using ZStack.AspNetCore.UnifyResult;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// 状态码中间件拓展
/// </summary>
public static class UnifyResultMiddlewareExtensions
{
    /// <summary>
    /// 添加状态码拦截中间件
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseUnifyResultStatusCodes(this IApplicationBuilder app)
    {
        app.UseMiddleware<UnifyResultStatusCodesMiddleware>();
        return app;
    }
}
