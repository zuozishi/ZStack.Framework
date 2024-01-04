using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StackExchange.Profiling;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace ZStack.AspNetCore;

/// <summary>
/// 全局应用类
/// </summary>
public partial class App
{
    /// <summary>
    /// 存储根服务，可能为空
    /// </summary>
    public static IServiceProvider? ServiceProvider => InternalApp.ServiceProvider;

    /// <summary>
    /// 应用日志记录器
    /// </summary>
    public static ILogger Logger => InternalApp.Logger;

    /// <summary>
    /// 全局配置选项
    /// </summary>
    public static IConfiguration? Configuration => InternalApp.Configuration;

    /// <summary>
    /// 获取泛型主机环境，如，是否是开发环境，生产环境等
    /// </summary>
    public static IHostEnvironment? HostEnvironment => InternalApp.HostEnvironment;

    /// <summary>
    /// 判断是否是单文件环境
    /// </summary>
    public static bool SingleFileEnvironment => string.IsNullOrWhiteSpace(Assembly.GetEntryAssembly()?.Location);

    /// <summary>
    /// 应用有效程序集
    /// </summary>
    public static IEnumerable<Assembly> Assemblies => InternalApp.Assemblies;

    /// <summary>
    /// 有效程序集类型
    /// </summary>
    public static IEnumerable<Type> EffectiveTypes => InternalApp.EffectiveTypes;

    /// <summary>
    /// 获取请求上下文
    /// </summary>
    public static HttpContext? HttpContext => InternalApp.ServiceProvider?.GetService<IHttpContextAccessor>()?.HttpContext;

    /// <summary>
    /// 组件列表
    /// </summary>
    public static ComponentContext[] Components => [.. InternalApp.Components];

    /// <summary>
    /// 获取选项
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <returns></returns>
    public static TOptions GetOptions<TOptions>() where TOptions : class, new()
        => InternalApp.GetOptions<TOptions>();

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static TService GetRequiredService<TService>() where TService : class
        => InternalApp.ServiceProvider!.GetRequiredService<TService>();

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object GetRequiredService(Type type)
        => InternalApp.ServiceProvider!.GetRequiredService(type);

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static TService? GetService<TService>() where TService : class
        => InternalApp.ServiceProvider?.GetService<TService>();

    /// <summary>
    /// 获取请求生存周期的服务
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object? GetService(Type type)
        => InternalApp.ServiceProvider?.GetService(type);

    /// <summary>
    /// 获取请求生存周期的服务集合
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TService> GetServices<TService>() where TService : class
        => InternalApp.ServiceProvider?.GetServices<TService>() ?? [];

    /// <summary>
    /// 获取请求生存周期的服务集合
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<object?> GetServices(Type type)
        => InternalApp.ServiceProvider?.GetServices(type) ?? [];

    /// <summary>
    /// 获取服务注册的生命周期类型
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    public static ServiceLifetime? GetServiceLifetime(Type serviceType)
        => InternalApp.GetServiceLifetime(serviceType);

    /// <summary>
    /// 打印验证信息到 MiniProfiler
    /// </summary>
    /// <param name="category">分类</param>
    /// <param name="state">状态</param>
    /// <param name="message">消息</param>
    /// <param name="isError">是否为警告消息</param>
    public static void PrintToMiniProfiler(string category, string state, string? message = null, bool isError = false)
    {
        if (!CanBeMiniProfiler()) return;

        // 打印消息
        var titleCaseCategory = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(category);
        var customTiming = MiniProfiler.Current?.CustomTiming(category, string.IsNullOrWhiteSpace(message) ? $"{titleCaseCategory} {state}" : message, state);
        if (customTiming == null) return;

        // 判断是否是警告消息
        if (isError) customTiming.Errored = true;
    }

    /// <summary>
    /// 判断是否启用 MiniProfiler
    /// </summary>
    /// <returns></returns>
    internal static bool CanBeMiniProfiler()
    {
        // 减少不必要的监听
        if (HttpContext == null
            || !(HttpContext.Request.Headers.TryGetValue("request-from", out var value) && value == "swagger")) return false;

        return true;
    }
}
