using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using ZStack.AspNetCore.Exceptions;

namespace ZStack.AspNetCore.UnifyResult;

/// <summary>
/// 规范化结果
/// </summary>
[UnifyModel(typeof(RESTfulResult<>))]
public class RESTfulResultProvider : IUnifyResultProvider
{
    public const string ActivityName = "RESTfulResultActivitySource";

    private static readonly ActivitySource _activitySource = new(ActivityName);

    /// <summary>
    /// 异常返回值
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public IActionResult OnException(ExceptionContext context)
    {
        using var activity = _activitySource.StartActivity("RESTfulResult.OnException");
        var ex = context.Exception;
        int statusCode = 500;
        if (ex is AppException exception)
            statusCode = exception.ErrorCode;
        else if (ex is BadHttpRequestException requestException)
            statusCode = requestException.StatusCode;
        activity?.SetTag("statusCode", statusCode);
        activity?.AddEvent(new ActivityEvent("Exception", default, new ActivityTagsCollection
        {
            { "code", statusCode },
            { "message", ex.Message },
            { "stackTrace", ex.StackTrace },
            { "source", ex.Source },
            { "helpLink", ex.HelpLink }
        }));
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
            Success = true,
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
        using var activity = _activitySource.StartActivity("RESTfulResult.OnValidateFailed");
        var errors = new List<string>();
        foreach (var item in context.ModelState)
        {
            foreach (var error in item.Value.Errors)
            {
                errors.Add($"[{item.Key}]{error.ErrorMessage}");
            }
        }
        var message = $"参数验证失败：{Environment.NewLine}{string.Join(Environment.NewLine, errors)}";
        activity?.SetTag("statusCode", 400);
        activity?.AddEvent(new ActivityEvent("ValidationInfo", default, new ActivityTagsCollection
        {
            { "code", 400 },
            { "message", message },
            { "modelState", context.ModelState.ToJson() }
        }));
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
