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
    /// 存储根服务
    /// </summary>
    public static IServiceProvider RootServices { get; } = FurionApp.RootServices;

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
}

internal class ZStackApp
{

}
