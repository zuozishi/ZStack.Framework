using ZStack.AspNetCore;
using ZStack.AspNetCore.SqlSugar;

namespace Microsoft.Extensions.DependencyInjection;

public static class SqlSugarSetup
{
    /// <summary>
    /// 添加SqlSugar及分布式Id生成器
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddZStackSqlSugarWithSnowIdGenerator(this IServiceCollection services)
    {
        services.AddZStackSqlSugar(App.Configuration!);
        services.AddSnowIdGenerator();
        return services;
    }

    /// <summary>
    /// 添加分布式Id生成器
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSnowIdGenerator(this IServiceCollection services)
    {
        return services.AddHostedService<IdGeneratorWorker>();
    }
}
