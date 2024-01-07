using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ZStack.AspNetCore.UnifyResult;

/// <summary>
/// 规范化结果过滤器
/// </summary>
public class UnifyResultFilter : IExceptionFilter, IActionFilter
{
    /// <summary>
    /// 成功返回值
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
            return;

        // 获取控制器信息
        if (context.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
            return;

        // 检查是否是有效的结果（可进行规范化的结果）
        if (UnifyContext.CheckVaildResult(context.Result, out var data)
            && !UnifyContext.CheckSucceededNonUnify(actionDescriptor.MethodInfo, out var unifyResult))
        {
            if (unifyResult != null)
                context.Result = unifyResult.OnSucceeded(context, data);
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        
    }

    /// <summary>
    /// 异常处理
    /// </summary>
    /// <param name="context"></param>
    public void OnException(ExceptionContext context)
    {
        if (!UnifyContext.CheckStatusCodeNonUnify(context.HttpContext, out var unifyResult))
        {
            if (unifyResult != null)
                context.Result = unifyResult.OnException(context);
        }
    }
}
