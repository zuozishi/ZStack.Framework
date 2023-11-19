using Furion.Schedule;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace ZStack.AspNetCore.Schedule.Dashboard;

public static class ScheduleUISetup
{
    public static IApplicationBuilder UseZStackScheduleUI(this IApplicationBuilder app, Action<ScheduleUIOptions>? configureAction = null)
    {
        var options = new ScheduleUIOptions();
        configureAction?.Invoke(options);

        // 生产环境关闭
        if (options.DisableOnProduction
            && app.ApplicationServices.GetRequiredService<IWebHostEnvironment>().IsProduction())
            return app;

        // 如果路由为空，或者不以 / 开头，或者以 / 结尾，不启动看板
        if (string.IsNullOrWhiteSpace(options.RequestPath) || !options.RequestPath.StartsWith('/') || options.RequestPath.EndsWith('/'))
            return app;

        // 注册 Schedule 中间件
        app.UseMiddleware<ScheduleUIMiddleware>(options);

        // 获取当前类型所在程序集
        var currentAssembly = typeof(ScheduleUISetup).Assembly;

        // 注册嵌入式文件服务器
        app.UseFileServer(new FileServerOptions
        {
            FileProvider = new EmbeddedFileProvider(currentAssembly, $"{currentAssembly.GetName().Name}.wwwroot"),
            RequestPath = options.RequestPath,
            EnableDirectoryBrowsing = options.EnableDirectoryBrowsing
        });

        return app;
    }
}
