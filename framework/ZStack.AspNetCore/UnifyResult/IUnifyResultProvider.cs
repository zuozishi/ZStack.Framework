﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Http;

namespace ZStack.AspNetCore.UnifyResult;

/// <summary>
/// 规范化结果提供器
/// </summary>
public interface IUnifyResultProvider
{
    /// <summary>
    /// 异常返回值
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    IActionResult OnException(ExceptionContext context);

    /// <summary>
    /// 成功返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    IActionResult OnSucceeded(ActionExecutedContext context, object? data);

    /// <summary>
    /// 验证失败返回值
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    IActionResult OnValidateFailed(ActionContext context);

    /// <summary>
    /// 拦截返回状态码
    /// </summary>
    /// <param name="context"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    Task OnResponseStatusCodes(HttpContext context, int statusCode);
}
