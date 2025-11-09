using Microsoft.Extensions.Configuration;
using ZStack.SqlSugar;
using ZStack.SqlSugar.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class SqlSugarSetup
{
    /// <summary>
    /// SqlSugar 上下文初始化
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddZStackSqlSugar(this IServiceCollection services, IConfiguration configuration)
        => AddZStackSqlSugar<DefaultSqlSugarInitializer>(services, configuration);

    /// <summary>
    /// SqlSugar 上下文初始化
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddZStackSqlSugar<TInitializer>(this IServiceCollection services, IConfiguration configuration)
        where TInitializer : class, ISqlSugarInitializer
    {
        services.AddZStackOptions<SnowIdOptions>();
        services.AddZStackOptions<DbConnectionOptions>();

        // 注册雪花Id
        var snowIdOptions = configuration.GetOtions<SnowIdOptions>();
        YitIdHelper.SetIdGenerator(snowIdOptions);
        // 自定义 SqlSugar 雪花ID算法
        SnowFlakeSingle.WorkId = snowIdOptions.WorkerId;
        StaticConfig.CustomSnowFlakeFunc = YitIdHelper.NextId;

        services.AddSingleton<ISqlSugarInitializer, TInitializer>();
        services.AddSingleton<ISqlSugarService, SqlSugarService>();
        services.AddSingleton<ISqlSugarClient>(sp => sp.GetRequiredService<ISqlSugarService>().Get());
        services.AddSingleton(typeof(DbRepository<>));

        return services;
    }
}
