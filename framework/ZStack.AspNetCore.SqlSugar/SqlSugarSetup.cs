namespace Microsoft.Extensions.DependencyInjection;

public static class SqlSugarSetup
{
    /// <summary>
    /// SqlSugar 上下文初始化
    /// </summary>
    /// <param name="services"></param>
    public static void AddZStackSqlSugar(this IServiceCollection services)
        => AddZStackSqlSugar<SqlSugarInitializer>(services);

    /// <summary>
    /// SqlSugar 上下文初始化
    /// </summary>
    /// <param name="services"></param>
    public static void AddZStackSqlSugar<TInitializer>(this IServiceCollection services)
        where TInitializer : class, ISqlSugarInitializer
    {
        ServiceCollectionExtensions.AddZStackOptions<SnowIdOptions>(services);
        ServiceCollectionExtensions.AddZStackOptions<DbConnectionOptions>(services);

        // 注册雪花Id
        YitIdHelper.SetIdGenerator(App.GetOptions<SnowIdOptions>());

        // 自定义 SqlSugar 雪花ID算法
        SnowFlakeSingle.WorkId = App.GetOptions<SnowIdOptions>().WorkerId;
        StaticConfig.CustomSnowFlakeFunc = () =>
        {
            return YitIdHelper.NextId();
        };

        services.AddSingleton<ISqlSugarInitializer, TInitializer>();
        services.AddSingleton<ISqlSugarService, SqlSugarService>();
        services.AddSingleton<ISqlSugarClient>(sp => sp.GetRequiredService<ISqlSugarService>().Get());
        services.AddSingleton(typeof(SqlSugarRepository<>));
    }
}
