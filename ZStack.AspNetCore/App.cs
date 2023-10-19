global using FurionApp = Furion.App;
using Furion;
using Microsoft.Extensions.DependencyInjection;

namespace ZStack.AspNetCore;

/// <summary>
/// 全局应用类
/// </summary>
public static class App
{
    #region 依赖注入容器
    /// <summary>
    /// 获取存储根服务
    /// </summary>
    /// <returns></returns>
    public static IServiceProvider GetRootServiceProvider()
        => FurionApp.RootServices;

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static TService GetRequiredService<TService>(IServiceProvider? serviceProvider = null) where TService: class
        => FurionApp.GetRequiredService<TService>(serviceProvider);

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static TService GetService<TService>(IServiceProvider? serviceProvider = null) where TService : class
        => FurionApp.GetService<TService>(serviceProvider);

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TService> GetServices<TService>(IServiceProvider? serviceProvider = null) where TService : class
        => FurionApp.GetServices<TService>(serviceProvider);

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="serviceKey"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static TService GetRequiredKeyedService<TService>(object? serviceKey, IServiceProvider? serviceProvider = null) where TService : class
        => (serviceProvider ?? FurionApp.RootServices).GetRequiredKeyedService<TService>(serviceKey);

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="serviceKey"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static TService? GetKeyedService<TService>(object? serviceKey, IServiceProvider? serviceProvider = null) where TService : class
        => (serviceProvider ?? FurionApp.RootServices).GetKeyedService<TService>(serviceKey);

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="serviceKey"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static IEnumerable<TService> GetKeyedServices<TService>(object? serviceKey, IServiceProvider? serviceProvider = null) where TService : class
        => (serviceProvider ?? FurionApp.RootServices).GetKeyedServices<TService>(serviceKey);
    #endregion

    /// <summary>
    /// 获取选项
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <returns></returns>
    public static TOptions GetOptions<TOptions>() where TOptions : class, new()
        => FurionApp.GetOptions<TOptions>();

    /// <summary>
    /// 应用全局配置
    /// </summary>
    public static AppSettingsOptions Settings => FurionApp.Settings;
}
