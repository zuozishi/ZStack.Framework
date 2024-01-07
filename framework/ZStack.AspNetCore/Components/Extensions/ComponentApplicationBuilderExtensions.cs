using Microsoft.AspNetCore.Hosting;
using ZStack.AspNetCore.Components;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// 组件应用中间件拓展类
/// </summary>
public static class ComponentApplicationBuilderExtensions
{
    /// <summary>
    /// 注册依赖组件
    /// </summary>
    /// <typeparam name="TComponent">派生自 <see cref="IApplicationComponent"/></typeparam>
    /// <param name="app"><see cref="IApplicationBuilder"/></param>
    /// <param name="options">组件参数</param>
    /// <returns><see cref="IApplicationBuilder"/></returns>
    public static IApplicationBuilder UseComponent<TComponent>(this IApplicationBuilder app, object? options = null)
        where TComponent : class, IApplicationComponent, new()
    {
        return app.UseComponent<TComponent, object?>(options);
    }

    /// <summary>
    /// 注册依赖组件
    /// </summary>
    /// <typeparam name="TComponent">派生自 <see cref="IApplicationComponent"/></typeparam>
    /// <typeparam name="TComponentOptions">组件参数</typeparam>
    /// <param name="app"><see cref="IApplicationBuilder"/></param>
    /// <param name="options">组件参数</param>
    /// <returns><see cref="IApplicationBuilder"/></returns>
    public static IApplicationBuilder UseComponent<TComponent, TComponentOptions>(this IApplicationBuilder app, TComponentOptions options)
        where TComponent : class, IApplicationComponent, new()
    {
        return app.UseComponent(typeof(TComponent), options);
    }

    /// <summary>
    /// 注册依赖组件
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/></param>
    /// <param name="componentType">组件类型</param>
    /// <param name="options">组件参数</param>
    /// <returns><see cref="IApplicationBuilder"/></returns>
    public static IApplicationBuilder UseComponent(this IApplicationBuilder app, Type componentType, object? options = null)
    {
        Penetrates.AddApplicationComponent(app, componentType, options);
        return app;
    }
}
