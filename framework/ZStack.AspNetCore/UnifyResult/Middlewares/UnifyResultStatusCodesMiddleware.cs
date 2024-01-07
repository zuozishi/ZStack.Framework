using Microsoft.AspNetCore.Http;

namespace ZStack.AspNetCore.UnifyResult;

public class UnifyResultStatusCodesMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);

        // 只有请求错误（短路状态码）和非 WebSocket 才支持规范化处理
        if (context.IsWebSocketRequest()
            || context.Response.StatusCode < 400
            || context.Response.StatusCode == 404) return;

        // 处理规范化结果
        if (!UnifyContext.CheckStatusCodeNonUnify(context, out var unifyResult))
        {
            // 解决刷新 Token 时间和 Token 时间相近问题
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized
                && context.Response.Headers.ContainsKey("access-token")
                && context.Response.Headers.ContainsKey("x-access-token"))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
            }

            // 如果 Response 已经完成输出，则禁止写入
            if (context.Response.HasStarted) return;

            if (unifyResult != null)
                await unifyResult.OnResponseStatusCodes(context, context.Response.StatusCode);
        }
    }
}
