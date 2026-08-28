namespace ZStackWebApi;

/// <summary>
/// 应用启动配置。
/// </summary>
[AppStartupAttribute(Order = 1)]
public sealed class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseAuthorization();
    }
}
