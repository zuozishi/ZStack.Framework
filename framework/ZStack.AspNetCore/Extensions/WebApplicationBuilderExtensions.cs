using ZStack.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// 服务注入
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="loggerConfigure"></param>
    /// <param name="autoLoadComponents">自动扫描注册组件</param>
    /// <param name="components">手动注册组件列表</param>
    /// <param name="ignoreComponents">忽略自动注册组件列表</param>
    /// <returns></returns>
    public static WebApplicationBuilder Inject(
        this WebApplicationBuilder builder,
        Action<LoggerConfiguration>? loggerConfigure = null,
        bool autoLoadComponents = true,
        Type[]? components = null,
        Type[]? ignoreComponents = null)
    {
        // 配置主机
        InternalApp.ConfigureHostApplication(builder);

        // 配置Serilog
        InternalApp.ConfigureSerilog(builder, loggerConfigure);

        // 装载服务组件
        builder.Services.AddComponents(autoLoadComponents, components, ignoreComponents);

        // 自动注入依赖
        builder.Services.AddAutoDependencyInjection();

        return builder;
    }
}
