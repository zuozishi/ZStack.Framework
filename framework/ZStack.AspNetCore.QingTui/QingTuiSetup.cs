using ZStack.AspNetCore.QingTui;
using ZStack.QingTui;

namespace Microsoft.Extensions.DependencyInjection;

public static class QingTuiSetup
{
    /// <summary>
    /// 注册轻推API客户端
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddQingTuiClient(this IServiceCollection services)
    {
        services.AddQingTuiClient<CacheTokenPersister>();
        return services;
    }

    /// <summary>
    /// 注册轻推API客户端
    /// </summary>
    /// <typeparam name="TokenPersister"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddQingTuiClient<TokenPersister>(this IServiceCollection services) where TokenPersister : class, ITokenPersister
    {
        services.AddZStackOptions<QingTuiClientOptions>();
        services.AddSingleton<ITokenPersister, TokenPersister>();
        services.AddSingleton<IQingTuiClientService, QingTuiClientService>();
        return services;
    }
}
