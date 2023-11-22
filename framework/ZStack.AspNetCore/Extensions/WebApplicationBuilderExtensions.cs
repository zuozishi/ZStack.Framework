namespace Microsoft.AspNetCore.Builder;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// 服务注入
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure"></param>
    /// <param name="loggerConfigure"></param>
    /// <param name="scan">自动扫描注册组件</param>
    /// <param name="components">手动注册组件列表</param>
    /// <param name="ignoreComponents">忽略自动注册组件列表</param>
    /// <returns></returns>
    public static WebApplicationBuilder InjectZStack(
        this WebApplicationBuilder builder,
        Action<WebApplicationBuilder, InjectOptions>? configure = null,
        Action<LoggerConfiguration>? loggerConfigure = null,
        bool scan = true,
        Type[]? components = null,
        Type[]? ignoreComponents = null)
    {
        // 注入Furion
        builder.Inject(configure);

        // 配置Serilog
        builder.UseZStackSerilog(loggerConfigure);

        // 装载组件
        builder.AddZStackComponents(scan, components, ignoreComponents);

        return builder;
    }

    /// <summary>
    /// 注册ZStack服务组件
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="scan">自动扫描注册组件</param>
    /// <param name="components">手动注册组件列表</param>
    /// <param name="ignoreComponents">忽略自动注册组件列表</param>
    /// <returns></returns>
    public static WebApplicationBuilder AddZStackComponents(this WebApplicationBuilder builder, bool scan = true, Type[]? components = null, Type[]? ignoreComponents = null)
    {
        var componentList = new List<Type>();
        components?.ForEach((type, _) =>
        {
            if (ignoreComponents != null && ignoreComponents.Contains(type))
                return;
            componentList.Add(type);
        });
        if (scan)
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
    /// 注册Serilog
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure"></param>
    public static void UseZStackSerilog(this WebApplicationBuilder builder, Action<LoggerConfiguration>? configure = null)
    {
        Log.Logger = SerilogLogger.CreateConfigurationLogger(builder.Configuration, configure);
        builder.Host.UseSerilog();
    }
}
