using ZStack.Core.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class DIServiceCollectionExtensions
{
    public static IServiceCollection AddAutoDependencyInjection(this IServiceCollection services)
    {
        DI.AutoAddServices(services);
        return services;
    }
}
