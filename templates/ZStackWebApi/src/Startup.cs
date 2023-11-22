namespace ZStackWebApi;

/// <summary>
/// 应用启动配置
/// </summary>
public class Startup : AppStartup
{
    /// <summary>
    /// 服务配置
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews()
            .AddNewtonsoftJson(options => SetNewtonsoftJsonSetting(options.SerializerSettings))
            .AddInjectWithUnifyResult<RestfulResultProvider>();
        services.AddCorsAccessor();
    }

    /// <summary>
    /// 中间件配置
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseRouting();

        app.UseCorsAccessor();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseZStackInject();
    }

    /// <summary>
    /// Json序列化设置
    /// </summary>
    /// <param name="setting"></param>
    private static void SetNewtonsoftJsonSetting(JsonSerializerSettings setting)
    {
        setting.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        setting.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        setting.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 时间格式化
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
        // setting.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 解决动态对象属性名大写
        // setting.NullValueHandling = NullValueHandling.Ignore; // 忽略空值
        // setting.Converters.AddLongTypeConverters(); // long转string（防止js精度溢出） 超过16位开启
        // setting.MetadataPropertyHandling = MetadataPropertyHandling.Ignore; // 解决DateTimeOffset异常
        // setting.DateParseHandling = DateParseHandling.None; // 解决DateTimeOffset异常
        // setting.Converters.Add(new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }); // 解决DateTimeOffset异常
    }
}
