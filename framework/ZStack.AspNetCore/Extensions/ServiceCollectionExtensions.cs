using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NewLife.Reflection;
using System.Reflection;
using ZStack.AspNetCore;
using ZStack.AspNetCore.Attributes;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册配置选项
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="services"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static IServiceCollection AddZStackOptions<TOptions>(this IServiceCollection services, string? sectionName = null) where TOptions : class, new()
    {
        var typeName = typeof(TOptions).Name;
        string _sectionName = sectionName ??
            typeof(TOptions).GetCustomAttribute<OptionsSectionAttribute>()?.Key ??
            (typeName.EndsWith("Options", StringComparison.OrdinalIgnoreCase) ? typeName[..^7] : typeName);
        services.AddOptions<TOptions>()
            .Bind(InternalApp.Configuration!.GetSection(_sectionName))
            .ValidateDataAnnotations();
        return services;
    }

    /// <summary>
    /// 自动注册服务组件
    /// </summary>
    /// <param name="services"></param>
    /// <param name="autoLoad"></param>
    /// <param name="components"></param>
    /// <param name="ignoreComponents"></param>
    /// <returns></returns>
    public static IServiceCollection AddComponents(this IServiceCollection services, bool autoLoad = true, Type[]? components = null, Type[]? ignoreComponents = null)
    {
        var componentList = new List<Type>();
        components?.ForEach((type, _) =>
        {
            if (ignoreComponents != null && ignoreComponents.Contains(type))
                return;
            componentList.Add(type);
        });
        if (autoLoad)
            App.EffectiveTypes
                .Where(t => (typeof(IServiceComponent).IsAssignableFrom(t)) && !t.IsInterface && !t.IsAbstract)
                .ForEach((type, _) =>
                {
                    if (componentList.Contains(type)) return;
                    if (ignoreComponents != null && ignoreComponents.Contains(type)) return;
                    componentList.Add(type);
                });
        componentList.ForEach((type, _) =>
        {
            App.Logger.Information("注册服务组件: {Component}", type);
            services.AddComponent(type);
        });
        return services;
    }

    /// <summary>
    /// 添加 Startup 自动扫描
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddStartups(this IServiceCollection services)
    {
        // 扫描所有继承 AppStartup 的类
        var startups = App.EffectiveTypes
            .Where(u => typeof(AppStartup).IsAssignableFrom(u) && u.IsClass && !u.IsAbstract && !u.IsGenericType)
            .OrderByDescending(u => GetStartupOrder(u));

        // 注册自定义 startup
        foreach (var type in startups)
        {
            var startup = Activator.CreateInstance(type) as AppStartup;
            InternalApp.AppStartups.Add(startup!);

            // 获取所有符合依赖注入格式的方法，如返回值void，且第一个参数是 IServiceCollection 类型
            var serviceMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(u => u.ReturnType == typeof(void)
                    && u.GetParameters().Length > 0
                    && u.GetParameters().First().ParameterType == typeof(IServiceCollection));

            if (!serviceMethods.Any()) continue;

            var argTypes = serviceMethods.First().GetParameters().Select(u => u.ParameterType).ToArray();
            var args = new List<object?>();
            foreach (var argType in argTypes)
            {
                if (argType == typeof(IServiceCollection))
                    args.Add(services);
                else if (argType == typeof(IConfiguration))
                    args.Add(InternalApp.Configuration);
                else if (argType == typeof(IHostEnvironment))
                    args.Add(InternalApp.HostEnvironment);
                else
                    args.Add(null);
            }

            // 自动安装属性调用
            foreach (var method in serviceMethods)
            {
                method.Invoke(startup, [.. args]);
            }
        }

        return services;
    }

    /// <summary>
    /// 获取 Startup 排序
    /// </summary>
    /// <param name="type">排序类型</param>
    /// <returns>int</returns>
    private static int GetStartupOrder(Type type)
        => type.GetCustomAttribute<AppStartupAttribute>(true)?.Order ?? 0;
}
