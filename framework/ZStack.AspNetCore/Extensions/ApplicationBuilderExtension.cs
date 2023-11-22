using Furion.SpecificationDocument;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Hosting;
using Knife4UIOptions = ZStack.AspNetCore.Options.Knife4UIOptions;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtension
{
    /// <summary>
    /// 中间件注入（带Swagger）
    /// </summary>
    /// <param name="app"></param>
    /// <param name="routePrefix">空字符串将为首页</param>
    /// <param name="configure"></param>
    /// <param name="scan">自动扫描注册组件</param>
    /// <param name="components">手动注册组件列表</param>
    /// <param name="ignoreComponents">忽略自动注册组件列表</param>
    /// <returns></returns>
    public static IApplicationBuilder UseZStackInject(
        this IApplicationBuilder app,
        string? routePrefix = null,
        Action<UseInjectOptions>? configure = null,
        bool scan = true,
        Type[]? components = null,
        Type[]? ignoreComponents = null)
    {
        var knife4UIOptions = App.GetOptions<Knife4UIOptions>();
        if (knife4UIOptions.Enabled)
        {
            foreach (var groupInfo in SpecificationDocumentBuilder.GetOpenApiGroups())
            {
                knife4UIOptions.SwaggerEndpoint("/" + groupInfo.RouteTemplate, groupInfo.Title);
            }
            app.UseKnife4UI(knife4UIOptions);
        }
        app.UseInject(routePrefix, configure);
        app.UseZStackComponents(scan, components, ignoreComponents);
        return app;
    }

    /// <summary>
    /// 注入基础中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="scan">自动扫描注册组件</param>
    /// <param name="components">手动注册组件列表</param>
    /// <param name="ignoreComponents">忽略自动注册组件列表</param>
    /// <returns></returns>
    public static IApplicationBuilder UseZStackInjectBase(this IApplicationBuilder app, bool scan = true, Type[]? components = null, Type[]? ignoreComponents = null)
    {
        app.UseInjectBase();
        app.UseZStackComponents(scan, components, ignoreComponents);
        return app;
    }

    /// <summary>
    /// 注册ZStack服务中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="scan">自动扫描注册组件</param>
    /// <param name="components">手动注册组件列表</param>
    /// <param name="ignoreComponents">忽略自动注册组件列表</param>
    /// <returns></returns>
    public static IApplicationBuilder UseZStackComponents(this IApplicationBuilder app, bool scan = true, Type[]? components = null, Type[]? ignoreComponents = null)
    {
        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        var componentList = new List<Type>();
        components?.ForEach((type, _) =>
        {
            if (ignoreComponents != null && ignoreComponents.Contains(type))
                return;
            componentList.Add(type);
        });
        if (scan)
            FurionApp.EffectiveTypes
            .Where(t => (typeof(IApplicationComponent).IsAssignableFrom(t)) && !t.IsInterface && !t.IsAbstract)
            .ForEach((type, _) =>
            {
                if (componentList.Contains(type)) return;
                if (ignoreComponents != null && ignoreComponents.Contains(type)) return;
                componentList.Add(type);
            });
        componentList.ForEach((type, _) =>
        {
            App.Logger.Information("注册ZStack中间件组件: {Component}", type);
            app.UseComponent(env, type);
        });
        return app;
    }
}
