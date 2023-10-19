using Furion;
using Serilog;
using ZStack.AspNetCore;
using ZStack.AspNetCore.Components;

namespace Microsoft.AspNetCore.Builder;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Web 应用注入
    /// </summary>
    /// <param name="webApplicationBuilder">Web应用构建器</param>
    /// <param name="configure"></param>
    /// <returns>WebApplicationBuilder</returns>
    public static WebApplicationBuilder InjectZStack(
        this WebApplicationBuilder builder,
        Action<WebApplicationBuilder, InjectOptions>? configure = null,
        Action<LoggerConfiguration>? loggerConfigure = null)
    {
        // 注入Furion
        builder.Inject(configure);
        // 配置Serilog
        builder.AddSerilogSetup(loggerConfigure);
        // 装载组件
        builder.AddComponent<CacheComponent>();
        return builder;
    }
}
