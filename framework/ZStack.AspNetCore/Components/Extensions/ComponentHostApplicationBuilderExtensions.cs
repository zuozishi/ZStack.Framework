using ZStack.AspNetCore.Components;

namespace Microsoft.Extensions.Hosting;

public static class ComponentHostApplicationBuilderExtensions
{
    /// <summary>
    /// 注册主机组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="IWebComponent"/></typeparam>
    /// <param name="builder"><see cref="IHostApplicationBuilder"/></param>
    /// <param name="options">组件参数</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddComponent<TComponent>(this IHostApplicationBuilder builder, object? options = null)
        where TComponent : class, IWebComponent, new()
    {
        return builder.AddComponent<TComponent, object?>(options);
    }

    /// <summary>
    /// 注册主机组件
    /// </summary>
    /// <typeparam name="TComponent"><see cref="IWebComponent"/></typeparam>
    /// <typeparam name="TComponentOptions">组件参数</typeparam>
    /// <param name="builder"><see cref="IHostApplicationBuilder"/></param>
    /// <param name="options">组件参数</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddComponent<TComponent, TComponentOptions>(this IHostApplicationBuilder builder, TComponentOptions options)
        where TComponent : class, IWebComponent, new()
    {
        return builder.AddComponent(typeof(TComponent), options);
    }

    /// <summary>
    /// 注册主机组件
    /// </summary>
    /// <param name="builder"><see cref="IHostApplicationBuilder"/></param>
    /// <param name="componentType">组件类型</param>
    /// <param name="options">组件参数</param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddComponent(this IHostApplicationBuilder builder, Type componentType, object? options = null)
    {
        Penetrates.AddWebComponent(builder, componentType, options);
        return builder;
    }
}
