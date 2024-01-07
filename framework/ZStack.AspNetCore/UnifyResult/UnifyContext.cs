using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ZStack.AspNetCore.UnifyResult;

public static class UnifyContext
{
    /// <summary>
    /// 是否启用规范化结果
    /// </summary>
    internal static bool EnabledUnifyHandler = false;

    /// <summary>
    /// 规范化结果额外数据键
    /// </summary>
    internal static string UnifyResultExtrasKey = "UNIFY_RESULT_EXTRAS";

    /// <summary>
    /// 规范化结果提供器
    /// </summary>
    internal static ConcurrentDictionary<string, UnifyMetadata> UnifyProviders = new();

    /// <summary>
    /// 跳过规范化处理的 Response Content-Type
    /// </summary>
    internal static string[] ResponseContentTypesOfNonUnify =
    [
        "text/event-stream",
        "application/pdf",
        "application/octet-stream",
        "image/"
    ];

    /// <summary>
    /// 填充附加信息
    /// </summary>
    /// <param name="extras"></param>
    public static void Fill(object extras)
    {
        var items = App.HttpContext?.Items;
        if (items != null && items.ContainsKey(UnifyResultExtrasKey))
            items.Remove(UnifyResultExtrasKey);
        items?.Add(UnifyResultExtrasKey, extras);
    }

    /// <summary>
    /// 读取附加信息
    /// </summary>
    public static object? Take()
    {
        object? extras = null;
        App.HttpContext?.Items?.TryGetValue(UnifyResultExtrasKey, out extras);
        return extras;
    }

    /// <summary>
    /// 检查是否是有效的结果（可进行规范化的结果）
    /// </summary>
    /// <param name="result"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static bool CheckVaildResult(IActionResult? result, out object? data)
    {
        data = default;

        // 排除以下结果，跳过规范化处理
        var isDataResult = result switch
        {
            ViewResult => false,
            PartialViewResult => false,
            FileResult => false,
            ChallengeResult => false,
            SignInResult => false,
            SignOutResult => false,
            RedirectToPageResult => false,
            RedirectToRouteResult => false,
            RedirectResult => false,
            RedirectToActionResult => false,
            LocalRedirectResult => false,
            ForbidResult => false,
            ViewComponentResult => false,
            PageResult => false,
            NotFoundResult => false,
            NotFoundObjectResult => false,
            _ => true,
        };

        // 目前支持返回值 ActionResult
        if (isDataResult)
            data = result switch
            {
                // 处理内容结果
                ContentResult content => content.Content,
                // 处理对象结果
                ObjectResult obj => obj.Value,
                // 处理 JSON 对象
                JsonResult json => json.Value,
                _ => null,
            };

        return isDataResult;
    }

    /// <summary>
    /// 获取方法规范化元数据
    /// </summary>
    /// <remarks>如果追求性能，这里理应缓存起来，避免每次请求去检测</remarks>
    /// <param name="method"></param>
    /// <returns></returns>
    internal static UnifyMetadata? GetMethodUnityMetadata(MethodInfo method)
    {
        var unityProviderAttribute = method.GetFoundAttribute<UnifyProviderAttribute>(true);

        // 获取元数据
        var isExists = UnifyProviders.TryGetValue(unityProviderAttribute?.Name ?? string.Empty, out var metadata);
        if (!isExists)
        {
            // 不存在则将默认的返回
            UnifyProviders.TryGetValue(string.Empty, out metadata);
        }

        return metadata;
    }

    /// <summary>
    /// 查找方法指定特性，如果没找到则继续查找声明类
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="method"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    internal static TAttribute? GetFoundAttribute<TAttribute>(this MethodInfo method, bool inherit)
        where TAttribute : Attribute
    {
        // 获取方法所在类型
        var declaringType = method.DeclaringType;

        var attributeType = typeof(TAttribute);

        // 判断方法是否定义指定特性，如果没有再查找声明类
        var foundAttribute = method.IsDefined(attributeType, inherit)
            ? method.GetCustomAttribute<TAttribute>(inherit)
            : (
                declaringType != null && declaringType.IsDefined(attributeType, inherit)
                ? declaringType.GetCustomAttribute<TAttribute>(inherit)
                : default
            );

        return foundAttribute;
    }

    /// <summary>
    /// 检查请求成功是否进行规范化处理
    /// </summary>
    /// <param name="method"></param>
    /// <param name="unifyResult"></param>
    /// <param name="isWebRequest"></param>
    /// <returns>返回 true 跳过处理，否则进行规范化处理</returns>
    internal static bool CheckSucceededNonUnify(MethodInfo method, out IUnifyResultProvider? unifyResult, bool isWebRequest = true)
    {
        // 解析规范化元数据
        var unityMetadata = GetMethodUnityMetadata(method);

        if (unityMetadata == null)
            return (unifyResult = null) == null;

        // 判断是否跳过规范化处理
        var isSkip = !EnabledUnifyHandler
              || method.GetRealReturnType().HasImplementedRawGeneric(unityMetadata.ResultType)
              || method.CustomAttributes.Any(x => typeof(NonUnifyAttribute).IsAssignableFrom(x.AttributeType) || typeof(ProducesResponseTypeAttribute).IsAssignableFrom(x.AttributeType) || typeof(IApiResponseMetadataProvider).IsAssignableFrom(x.AttributeType))
              || (method.ReflectedType?.IsDefined(typeof(NonUnifyAttribute), true) == true)
              || (method.DeclaringType?.Assembly.GetName().Name?.StartsWith("Microsoft.AspNetCore.OData") == true);

        if (!isWebRequest)
        {
            unifyResult = null;
            return isSkip;
        }

        if (unityMetadata != null)
        {
            unifyResult = isSkip ? null : App.GetService(unityMetadata.ProviderType) as IUnifyResultProvider;
            return unifyResult == null;
        }

        return (unifyResult = null) == null;
    }

    /// <summary>
    /// 检查短路状态码（>=400）是否进行规范化处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="unifyResult"></param>
    /// <returns>返回 true 跳过处理，否则进行规范化处理</returns>
    internal static bool CheckStatusCodeNonUnify(HttpContext context, out IUnifyResultProvider? unifyResult)
    {
        // 获取终点路由特性
        var endpointFeature = context.Features.Get<IEndpointFeature>();
        if (endpointFeature == null)
            return (unifyResult = null) == null;

        // 判断是否跳过规范化处理
        var isSkip = !EnabledUnifyHandler
                || context.GetMetadata<NonUnifyAttribute>() != null
                || endpointFeature?.Endpoint?.Metadata?.GetMetadata<NonUnifyAttribute>() != null
                || context.Request.Headers.Accept.ToString().Contains("odata.metadata=", StringComparison.OrdinalIgnoreCase)
                || context.Request.Headers.Accept.ToString().Contains("odata.streaming=", StringComparison.OrdinalIgnoreCase)
                || ResponseContentTypesOfNonUnify.Any(u => context.Response.Headers.ContentType.ToString().Contains(u, StringComparison.OrdinalIgnoreCase));

        if (isSkip == true)
            unifyResult = null;
        else
        {
            // 解析规范化元数据
            var unifyProviderAttribute = endpointFeature?.Endpoint?.Metadata?.GetMetadata<UnifyProviderAttribute>();
            UnifyProviders.TryGetValue(unifyProviderAttribute?.Name ?? string.Empty, out var unityMetadata);

            if (unityMetadata != null)
            {
                unifyResult = context.RequestServices.GetService(unityMetadata.ProviderType) as IUnifyResultProvider;
                return unifyResult == null;
            }
        }

        return (unifyResult = null) == null;
    }
}
