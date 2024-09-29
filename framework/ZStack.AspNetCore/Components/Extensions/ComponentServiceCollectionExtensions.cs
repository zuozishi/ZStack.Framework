using ZStack.AspNetCore.Components;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 组件应用服务集合拓展类
/// </summary>
public static class ComponentServiceCollectionExtensions
{
    /// <summary>
    /// 注册依赖组件
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddComponent<TComponent>(this IServiceCollection services, object? options = null)
        where TComponent : class, IServiceComponent, new()
    {
        return services.AddComponent<TComponent, object?>(options);
    }

    /// <summary>
    /// 注册依赖组件
    /// </summary>
    /// <typeparam name="TComponent">派生自 <see cref="IServiceComponent"/></typeparam>
    /// <typeparam name="TComponentOptions">组件参数</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="options">组件参数</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponent<TComponent, TComponentOptions>(this IServiceCollection services, TComponentOptions options)
        where TComponent : class, IServiceComponent, new()
    {
        return services.AddComponent(typeof(TComponent), options);
    }

    /// <summary>
    /// 注册依赖组件
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="componentType">组件类型</param>
    /// <param name="options">组件参数</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddComponent(this IServiceCollection services, Type componentType, object? options = null)
    {
        Penetrates.AddServiceComponent(services, componentType, options);
        return services;
    }
}
