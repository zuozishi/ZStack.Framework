using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZStack.AspNetCore.UnifyResult;

namespace ZStackWebApplication;

public class GlobalExceptionFilter : IExceptionFilter, IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        
    }

    public void OnException(ExceptionContext context)
    {
        var userAgent = context.HttpContext.Request.Headers.UserAgent;
        if (userAgent.ToString().Contains("Mozilla"))
            return;
        var result = new JsonResult(new RESTfulResult<object?>
        {
            Code = 500,
            Success = false,
            Result = null,
            Message = context.Exception.Message,
            Extras = new
            {
                context.Exception.StackTrace,
                context.Exception.Source,
                context.Exception.HelpLink,
            },
        });
        context.Result = result;
    }
}
