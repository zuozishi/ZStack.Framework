using Microsoft.AspNetCore.Hosting;

namespace Microsoft.AspNetCore.Builder;

public static class ZStackSetup
{
    /// <summary>
    /// 服务注入
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure"></param>
    /// <param name="loggerConfigure"></param>
    /// <returns></returns>
    public static WebApplicationBuilder InjectZStack(
        this WebApplicationBuilder builder,
        Action<WebApplicationBuilder, InjectOptions>? configure = null,
        Action<LoggerConfiguration>? loggerConfigure = null,
        Type[]? components = null,
        Type[]? ignoreComponents = null)
    {
        // 注入Furion
        builder.Inject(configure);

        // 配置Serilog
        builder.AddZStackSerilog(loggerConfigure);

        // 装载组件
        builder.AddZStackComponents(components, ignoreComponents);

        return builder;
    }

    /// <summary>
    /// 中间件注入
    /// </summary>
    /// <param name="app"></param>
    /// <param name="routePrefix"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseZStackInject(this IApplicationBuilder app, string? routePrefix = null, Action<UseInjectOptions>? configure = null)
    {
        app.UseInject(routePrefix, configure);
        app.UseZStackComponents();
        return app;
    }

    /// <summary>
    /// 注册Serilog
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure"></param>
    public static void AddZStackSerilog(this WebApplicationBuilder builder, Action<LoggerConfiguration>? configure = null)
    {
        Log.Logger = SerilogLogger.CreateConfigurationLogger(builder.Configuration, configure);
        builder.Host.UseSerilog();
    }

    /// <summary>
    /// 注册ZStack服务组件
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="components"></param>
    /// <param name="ignoreComponents"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddZStackComponents(this WebApplicationBuilder builder, Type[]? components = null, Type[]? ignoreComponents = null)
    {
        var componentList = new List<Type>();
        components?.ForEach((type, _) =>
        {
            if (ignoreComponents != null && ignoreComponents.Contains(type))
                return;
            componentList.Add(type);
        });
        FurionApp.EffectiveTypes
            .Where(t => (typeof(IServiceComponent).IsAssignableFrom(t)) && !t.IsInterface && !t.IsAbstract)
            .ForEach((type, _) =>
            {
                if (componentList.Contains(type)) return;
                if (ignoreComponents != null && ignoreComponents.Contains(type)) return;
                componentList.Add(type);
            });

        componentList.ForEach((type, _) =>
        {
            App.Logger.Information("注册ZStack服务组件: {Component}", type);
            builder.AddComponent(type);
        });
        return builder;
    }

    /// <summary>
    /// 注册ZStack服务中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="components"></param>
    /// <param name="ignoreComponents"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseZStackComponents(this IApplicationBuilder app, Type[]? components = null, Type[]? ignoreComponents = null)
    {
        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        var componentList = new List<Type>();
        components?.ForEach((type, _) =>
        {
            if (ignoreComponents != null && ignoreComponents.Contains(type))
                return;
            componentList.Add(type);
        });
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
