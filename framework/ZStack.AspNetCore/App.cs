using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Claims;

namespace ZStack.AspNetCore;

/// <summary>
/// 全局应用类
/// </summary>
public static class App
{
    /// <summary>
    /// 存储根服务，可能为空
    /// </summary>
    public static IServiceProvider RootServices { get; internal set; } = FurionApp.RootServices;

    /// <summary>
    /// 应用全局配置
    /// </summary>
    public static AppSettingsOptions Settings => FurionApp.Settings;

    /// <summary>
    /// 应用日志记录器
    /// </summary>
    public static ILogger Logger { get; } = Log.Logger.ForContext<ZStackApp>();

    /// <summary>
    /// 获取请求上下文用户
    /// </summary>
    public static ClaimsPrincipal? User => FurionApp.User;

    /// <summary>
    /// 全局配置选项
    /// </summary>
    public static IConfiguration Configuration => FurionApp.Configuration;

    /// <summary>
    /// 获取Web主机环境，如，是否是开发环境，生产环境等
    /// </summary>
    public static IWebHostEnvironment WebHostEnvironment => FurionApp.WebHostEnvironment;

    /// <summary>
    /// 获取泛型主机环境，如，是否是开发环境，生产环境等
    /// </summary>
    public static IHostEnvironment HostEnvironment => FurionApp.HostEnvironment;

    /// <summary>
    /// 判断是否是单文件环境
    /// </summary>
    public static bool SingleFileEnvironment => string.IsNullOrWhiteSpace(Assembly.GetEntryAssembly()?.Location);

    /// <summary>
    /// 应用有效程序集
    /// </summary>
    public static IEnumerable<Assembly> Assemblies => FurionApp.Assemblies;

    /// <summary>
    /// 有效程序集类型
    /// </summary>
    public static IEnumerable<Type> EffectiveTypes => FurionApp.EffectiveTypes;

    /// <summary>
    /// 获取请求上下文
    /// </summary>
    public static HttpContext? HttpContext => FurionApp.HttpContext;

    /// <summary>
    /// 未托管的对象集合
    /// </summary>
    public static ConcurrentBag<IDisposable> UnmanagedObjects => FurionApp.UnmanagedObjects;

    /// <summary>
    /// 获取选项
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <returns></returns>
    public static TOptions GetOptions<TOptions>(IServiceProvider? serviceProvider = null) where TOptions : class, new()
        => FurionApp.GetOptions<TOptions>(serviceProvider);

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static TService GetRequiredService<TService>(IServiceProvider? serviceProvider = null) where TService : class
        => FurionApp.GetRequiredService<TService>(serviceProvider);

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object GetRequiredService(Type type)
        => FurionApp.GetRequiredService(type);

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static TService? GetService<TService>(IServiceProvider? serviceProvider = null) where TService : class
        => FurionApp.GetService<TService>(serviceProvider);

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object? GetService(Type type, IServiceProvider? serviceProvider = null)
        => FurionApp.GetService(type, serviceProvider);

    /// <summary>
    /// 获取请求生存周期的服务集合
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TService> GetServices<TService>(IServiceProvider? serviceProvider = null) where TService : class
        => FurionApp.GetServices<TService>(serviceProvider);

    /// <summary>
    /// 获取请求生存周期的服务集合
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<object> GetServices(Type type, IServiceProvider? serviceProvider = null)
        => FurionApp.GetServices(type, serviceProvider);

    /// <summary>
    /// 获取服务注册的生命周期类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ServiceLifetime? GetServiceLifetime(Type serviceType)
        => FurionApp.GetServiceLifetime(serviceType);

    /// <summary>
    /// 解析服务提供器
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    public static IServiceProvider? GetServiceProvider(Type serviceType)
        => FurionApp.GetServiceProvider(serviceType);
}

internal class ZStackApp
{

}
