using System.Reflection;
using ZStack.AspNetCore;
using ZStack.AspNetCore.Attributes;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
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
}
