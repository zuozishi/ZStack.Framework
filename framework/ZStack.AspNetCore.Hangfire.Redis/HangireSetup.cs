using Hangfire;
using Hangfire.Redis.StackExchange;
using ZStack.AspNetCore;
using ZStack.AspNetCore.Hangfire.Redis;

namespace Microsoft.Extensions.DependencyInjection;

public static class HangireSetup
{
    public static IServiceCollection AddZStackHangireRedisStorage(
    this IServiceCollection services,
        Action<IGlobalConfiguration>? configuration = null)
    {
        services.AddZStackOptions<HangfireOptions>();
        var options = App.GetOptions<HangfireOptions>();
        services.AddZStackHangire(options, config =>
        {
            config.UseRedisStorage(options.ConnectionString, options.Redis);
            configuration?.Invoke(config);
        });
        return services;
    }
}
