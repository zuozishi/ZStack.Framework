using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ZStack.AspNetCore.Components;

namespace ZStack.AspNetCore.UnifyResult;

/// <summary>
/// 统一结果控制器应用模型转换器
/// </summary>
internal class UnifyResultModelConvention : IApplicationModelConvention
{
    /// <summary>
    /// 配置应用模型信息
    /// </summary>
    /// <param name="application"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            var type = controller.ControllerType;

            // 排除 OData 控制器
            if (type.Assembly.GetName().Name?.StartsWith("Microsoft.AspNetCore.OData") == true)
                continue;

            // 不能是非公开、基元类型、值类型、抽象类、接口、泛型类
            if (!type.IsPublic || type.IsPrimitive || type.IsValueType || type.IsAbstract || type.IsInterface || type.IsGenericType)
                continue;

            // 继承 ControllerBase 的类型 或 贴了 [ApiController] 特性
            if ((!typeof(Controller).IsAssignableFrom(type) && typeof(ControllerBase).IsAssignableFrom(type)) || typeof(ApiControllerAttribute).IsAssignableFrom(type))
            {
                // 控制器默认处理规范化结果
                if (UnifyContext.EnabledUnifyHandler)
                {
                    foreach (var action in controller.Actions)
                    {
                        ConfigureActionUnifyResultAttribute(action);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 配置规范化结果类型
    /// </summary>
    /// <param name="action"></param>
    private static void ConfigureActionUnifyResultAttribute(ActionModel action)
    {
        // 判断是否手动添加了标注或跳过规范化处理
        if (UnifyContext.CheckSucceededNonUnify(action.ActionMethod, out var _, false)) return;

        // 获取真实类型
        var returnType = action.ActionMethod.GetRealReturnType();
        if (returnType == typeof(void)) return;

        // 添加规范化结果特性
        action.Filters.Add(new UnifyResultAttribute(returnType, StatusCodes.Status200OK, action.ActionMethod));
    }
}
