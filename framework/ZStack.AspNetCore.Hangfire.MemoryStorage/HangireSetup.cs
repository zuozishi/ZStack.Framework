using Hangfire;
using Hangfire.MemoryStorage;
using ZStack.AspNetCore;
using ZStack.AspNetCore.Hangfire.MemoryStorage;

namespace Microsoft.Extensions.DependencyInjection;

public static class HangireSetup
{
    public static IServiceCollection AddZStackHangireMemoryStorage(
        this IServiceCollection services,
        Action<IGlobalConfiguration>? configuration = null)
    {
        ServiceCollectionExtensions.AddZStackOptions<HangfireOptions>(services);
        var options = App.GetOptions<HangfireOptions>();
        services.AddZStackHangire(options, config =>
        {
            config.UseMemoryStorage(options.MemoryStorage);
            configuration?.Invoke(config);
        });
        return services;
    }
}
