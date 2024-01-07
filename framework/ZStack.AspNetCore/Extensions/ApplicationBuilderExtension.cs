using ZStack.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtension
{
    /// <summary>
    /// 中间件注入（带Swagger）
    /// </summary>
    /// <param name="app"></param>
    /// <param name="autoLoadComponents">自动扫描注册组件</param>
    /// <param name="components">手动注册组件列表</param>
    /// <param name="ignoreComponents">忽略自动注册组件列表</param>
    /// <returns></returns>
    public static IApplicationBuilder UseZStackInject(
        this IApplicationBuilder app,
        bool autoLoadComponents = true,
        Type[]? components = null,
        Type[]? ignoreComponents = null)
    {
        InternalApp.ServiceProvider = app.ApplicationServices;
        app.UseComponents(autoLoadComponents, components, ignoreComponents);
        return app;
    }

    /// <summary>
    /// 自动注册中间件组件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="autoLoadComponents">自动扫描注册组件</param>
    /// <param name="components">手动注册组件列表</param>
    /// <param name="ignoreComponents">忽略自动注册组件列表</param>
    /// <returns></returns>
    public static IApplicationBuilder UseComponents(this IApplicationBuilder app, bool autoLoadComponents = true, Type[]? components = null, Type[]? ignoreComponents = null)
    {
        var componentList = new List<Type>();
        components?.ForEach((type, _) =>
        {
            if (ignoreComponents != null && ignoreComponents.Contains(type))
                return;
            componentList.Add(type);
        });
        if (autoLoadComponents)
            App.EffectiveTypes
                .Where(t => (typeof(IApplicationComponent).IsAssignableFrom(t)) && !t.IsInterface && !t.IsAbstract)
                .ForEach((type, _) =>
                {
                    if (componentList.Contains(type)) return;
                    if (ignoreComponents != null && ignoreComponents.Contains(type)) return;
                    componentList.Add(type);
                });
        componentList.ForEach((type, _) =>
        {
            App.Logger.Information("注册中间件组件: {Component}", type);
            app.UseComponent(type);
        });
        return app;
    }
}
