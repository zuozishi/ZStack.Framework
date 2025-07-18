using System.Reflection;
using ZStack.Core.Attributes;
using ZStack.Core.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加自动依赖注入
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAutoDependencyInjection(this IServiceCollection services)
    {
        DI.AutoAddServices(services);
        return services;
    }

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
            .BindConfiguration(_sectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return services;
    }
}
