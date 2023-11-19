using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.PostgreSql.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore.Hangfire.PostgreSql;

public static class HangireSetup
{
    public static IServiceCollection AddZStackHangirePostgreSqlStorage(
    this IServiceCollection services,
        Action<IGlobalConfiguration>? configuration = null)
    {
        services.AddConfigurableOptions<HangfireOptions>();
        var options = App.GetOptions<HangfireOptions>();
        services.AddZStackHangire(options, config =>
        {
            config.UsePostgreSqlStorage(con =>
            {
                con.UseConnectionFactory(new NpgsqlConnectionFactory(options.ConnectionString, options.PostgreSql));
            }, options.PostgreSql);
            configuration?.Invoke(config);
        });
        return services;
    }
}
