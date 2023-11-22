using Furion.FriendlyException;
using Furion.UnifyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Furion.DataValidation;

namespace ZStack.AspNetCore.Restful;

/// <summary>
/// 规范化结果
/// </summary>
[UnifyModel(typeof(RESTfulResult<>))]
public class RestfulResultProvider : IUnifyResultProvider
{
    /// <summary>
    /// 异常返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnException(ExceptionContext context, ExceptionMetadata metadata)
    {
        return new JsonResult(GetResult(metadata.StatusCode, false, metadata.Data, metadata.Errors));
    }

    /// <summary>
    /// 成功返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public IActionResult OnSucceeded(ActionExecutedContext context, object data)
    {
        return new JsonResult(GetResult(StatusCodes.Status200OK, true, data, null));
    }

    /// <summary>
    /// 验证失败返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public IActionResult OnValidateFailed(ActionExecutingContext context, ValidationMetadata metadata)
    {
        if (metadata.ValidationResult is string errMsg)
            return new JsonResult(new RESTfulResult<object>
            {
                Code = StatusCodes.Status400BadRequest,
                Message = errMsg,
                Success = false,
            });
        var message = new List<string>();
        var result = metadata.ValidationResult as Dictionary<string, string[]>;
        foreach (var type in result ?? [])
        {
            foreach (var value in type.Value)
            {
                message.Add($"[{type.Key}]{value}");
            }
        }
        return new JsonResult(new RESTfulResult<object>
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "数据验证失败：" + string.Join(", ", message),
            Success = false,
            Extras = metadata.ValidationResult,
        });
    }

    /// <summary>
    /// 特定状态码返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="statusCode"></param>
    /// <param name="unifyResultSettings"></param>
    /// <returns></returns>
    public async Task OnResponseStatusCodes(HttpContext context, int statusCode, UnifyResultSettingsOptions unifyResultSettings)
    {
        // 设置响应状态码
        UnifyContext.SetResponseStatusCodes(context, statusCode, unifyResultSettings);

        switch (statusCode)
        {
            // 处理 401 状态码
            case StatusCodes.Status401Unauthorized:
                await context.Response.WriteAsJsonAsync(GetResult(statusCode, false, null, "请先登录"),
                    App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
                break;
            // 处理 403 状态码
            case StatusCodes.Status403Forbidden:
                await context.Response.WriteAsJsonAsync(GetResult(statusCode, false, null, "禁止访问，没有权限"),
                    App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
                break;

            default: break;
        }
    }

    private static RESTfulResult<object> GetResult(int statusCode, bool isSuccess, object? data, object? errors)
    {
        return new RESTfulResult<object>
        {
            Code = statusCode,
            Success = isSuccess,
            Result = data,
            Message = isSuccess ? "success" : errors is null or string ? (errors + "") : errors?.ToJson() ?? string.Empty,
            Extras = UnifyContext.Take(),
        };
    }
}
