using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZStack.AspNetCore.Exceptions;

namespace ZStack.AspNetCore.UnifyResult;

/// <summary>
/// 规范化结果
/// </summary>
[UnifyModel(typeof(RESTfulResult<>))]
public class RESTfulResultProvider : IUnifyResultProvider
{
    /// <summary>
    /// 异常返回值
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public IActionResult OnException(ExceptionContext context)
    {
        var ex = context.Exception;
        int statusCode = 500;
        if (ex is AppException exception)
            statusCode = exception.ErrorCode;
        else if (ex is BadHttpRequestException requestException)
            statusCode = requestException.StatusCode;
        return new JsonResult(new RESTfulResult<object>
        {
            Code = statusCode,
            Success = false,
            Result = null,
            Message = ex.Message,
            Extras = new
            {
                ex.StackTrace,
                ex.Source,
                ex.HelpLink,
            },
            TraceId = context.HttpContext.TraceIdentifier
        });
    }

    /// <summary>
    /// 成功返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IActionResult OnSucceeded(ActionExecutedContext context, object? data)
    {
        return new JsonResult(new RESTfulResult<object>
        {
            Code = 200,
            Success = false,
            Result = data,
            Message = "success",
            Extras = UnifyContext.Take(),
            TraceId = context.HttpContext.TraceIdentifier
        });
    }

    /// <summary>
    /// 验证失败返回值
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IActionResult OnValidateFailed(ActionContext context)
    {
        var errors = new List<string>();
        foreach (var item in context.ModelState)
        {
            foreach (var error in item.Value.Errors)
            {
                errors.Add($"[{item.Key}]{error.ErrorMessage}");
            }
        }
        return new JsonResult(new RESTfulResult<object>
        {
            Code = 400,
            Success = false,
            Result = null,
            Message = $"参数验证失败：{Environment.NewLine}{string.Join(Environment.NewLine, errors)}",
            Extras = context.ModelState,
            TraceId = context.HttpContext.TraceIdentifier
        });
    }

    /// <summary>
    /// 设置状态码
    /// </summary>
    /// <param name="context"></param>
    /// <param name="statusCode"></param>
    public async Task OnResponseStatusCodes(HttpContext context, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        switch (statusCode)
        {
            // 处理 401 状态码
            case StatusCodes.Status401Unauthorized:
                await context.Response.WriteAsJsonAsync(new RESTfulResult<object>
                {
                    Code = statusCode,
                    Success = false,
                    Result = null,
                    Message = "请先登录",
                    Extras = null,
                    TraceId = context.TraceIdentifier
                });
                break;
            // 处理 403 状态码
            case StatusCodes.Status403Forbidden:
                await context.Response.WriteAsJsonAsync(new RESTfulResult<object>
                {
                    Code = statusCode,
                    Success = false,
                    Result = null,
                    Message = "禁止访问，没有权限",
                    Extras = null,
                    TraceId = context.TraceIdentifier
                });
                break;
            default: break;
        }
    }
}
