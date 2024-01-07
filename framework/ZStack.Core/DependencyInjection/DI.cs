using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;
using ZStack.Core.Utils;

namespace ZStack.Core.DependencyInjection;

/// <summary>
/// 依赖注入
/// </summary>
public static class DI
{
    /// <summary>
    /// 创建控制台应用程序依赖注入容器
    /// </summary>
    /// <param name="servicesConfigure"></param>
    /// <param name="addLogger"></param>
    /// <param name="loggerConfigure"></param>
    /// <returns></returns>
    public static IServiceProvider CreateConsoleAppServiceProvider(
        Action<ServiceCollection> servicesConfigure,
        bool addLogger = true,
        Action<LoggerConfiguration>? loggerConfigure = null)
    {
        var services = new ServiceCollection();
        if (addLogger)
        {
            var logger = SerilogLogger.CreateConsoleAppLogger(loggerConfigure);
            services.AddSingleton<ILogger>(logger);
            services.AddLogging(configure => configure.AddSerilog(logger));
        }
        servicesConfigure(services);
        services.AddAutoDependencyInjection();
        return services.BuildServiceProvider();
    }

    /// <summary>
    /// 自动配置服务
    /// </summary>
    /// <param name="services"></param>
    public static void AutoAddServices(IServiceCollection services)
    {
        var types = Reflection.GetTypes();
        var injectTypes = types
            .Where(x => x.IsAssignableTo(typeof(IPrivateDependency)) && !x.IsAbstract)
            .OrderBy(x => x.GetCustomAttribute<InjectionAttribute>()?.Order ?? 0)
            .ToArray();
        void addService(Type type, object? key, Type? implType)
        {
            var lifetime = ServiceLifetime.Scoped;
            if (type.IsAssignableTo(typeof(ISingleton)))
                lifetime = ServiceLifetime.Singleton;
            else if (type.IsAssignableTo(typeof(IScoped)))
                lifetime = ServiceLifetime.Scoped;
            else if (type.IsAssignableTo(typeof(ITransient)))
                lifetime = ServiceLifetime.Transient;
            else
                throw new Exception($"未知的生命周期 {type.FullName}");
            if (key is null)
                services.Add(new ServiceDescriptor(type, implType ?? type, lifetime));
            else
                services.Add(new ServiceDescriptor(type, key, implType ?? type, lifetime));
        }
        foreach (var type in injectTypes)
        {
            var injectAttr = type.GetCustomAttribute<InjectionAttribute>() ?? new();
            if (injectAttr.Action == InjectionActions.TryAdd && services.Any(x => x.ServiceType == type))
                continue;
            var interfances = type.GetInterfaces();
            switch (injectAttr.Pattern)
            {
                case InjectionPatterns.Self:
                    addService(type, injectAttr.Key, null);
                    break;
                case InjectionPatterns.FirstInterface:
                    if (interfances.Length == 0)
                        throw new Exception($"类型 {type.FullName} 没有实现接口");
                    addService(interfances.First(), injectAttr.Key, type);
                    break;
                case InjectionPatterns.SelfWithFirstInterface:
                    addService(type, injectAttr.Key, null);
                    if (interfances.Length == 0)
                        throw new Exception($"类型 {type.FullName} 没有实现接口");
                    addService(interfances.First(), injectAttr.Key, type);
                    break;
                case InjectionPatterns.ImplementedInterfaces:
                    foreach (var interfance in interfances)
                        if (!injectAttr.ExceptInterfaces.Contains(interfance))
                            addService(interfance, injectAttr.Key, type);
                    break;
                case InjectionPatterns.All:
                    addService(type, injectAttr.Key, null);
                    foreach (var interfance in interfances)
                        if (!injectAttr.ExceptInterfaces.Contains(interfance))
                            addService(interfance, injectAttr.Key, type);
                    break;
                default:
                    throw new Exception($"未知的注册模式 {injectAttr.Pattern}");
            }
        }
    }
}
