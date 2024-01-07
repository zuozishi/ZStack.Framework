using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;
using ZStack.AspNetCore.Attributes;
using ZStack.Core.Utils;
using ILogger = Serilog.ILogger;

namespace ZStack.AspNetCore;

internal static class InternalApp
{
    /// <summary>
    /// 主机环境
    /// </summary>
    internal static IHostEnvironment? HostEnvironment;

    /// <summary>
    /// 配置
    /// </summary>
    internal static IConfiguration? Configuration;

    /// <summary>
    /// 服务容器
    /// </summary>
    internal static IServiceCollection? ServiceDescriptors;

    /// <summary>
    /// 服务提供器
    /// </summary>
    internal static IServiceProvider? ServiceProvider;

    /// <summary>
    /// 应用日志记录器
    /// </summary>
    internal static ILogger Logger { get; private set; } = Log.Logger.ForContext<App>();

    /// <summary>
    /// 程序集
    /// </summary>
    internal static IEnumerable<Assembly> Assemblies { get; private set; } = Reflection.GetAssemblies();

    /// <summary>
    /// 程序集类型
    /// </summary>
    internal static IEnumerable<Type> EffectiveTypes { get; private set; } = Assemblies.SelectMany(asm => asm.GetTypes());

    /// <summary>
    /// 组件列表
    /// </summary>
    internal static List<ComponentContext> Components { get; } = [];

    /// <summary>
    /// 配置主机应用
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="loggerConfiguration"></param>
    internal static void ConfigureHostApplication(WebApplicationBuilder builder,
        Action<LoggerConfiguration>? loggerConfiguration = null)
    {
        ConfigureSerilog(builder, loggerConfiguration);
        ConfigureHostEnvironment(builder.Environment);
        ConfigureConfiguration(builder.Environment, builder.Configuration);
        ConfigureServices(builder.Services);
    }

    /// <summary>
    /// 配置Serilog
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure"></param>
    internal static void ConfigureSerilog(WebApplicationBuilder builder, Action<LoggerConfiguration>? configure = null)
    {
        Log.Logger = SerilogLogger.CreateConfigurationLogger(builder.Configuration, configure);
        Logger = Log.Logger.ForContext<App>();
        builder.Host.UseSerilog(Log.Logger);
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="services"></param>
    internal static void ConfigureServices(IServiceCollection services)
    {
        ServiceDescriptors = services;
    }

    /// <summary>
    /// 配置主机环境
    /// </summary>
    /// <param name="environment"></param>
    internal static void ConfigureHostEnvironment(IHostEnvironment environment)
    {
        HostEnvironment = environment;
    }

    /// <summary>
    /// 配置选项
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="configuration"></param>
    internal static void ConfigureConfiguration(IHostEnvironment environment, IConfigurationManager configuration)
    {
        Configuration = configuration;
        var env = environment.EnvironmentName;
        var configurationDirectory = configuration["ConfigurationDirectory"]
            ?? "Configuration";
        var files = new Matcher()
            .AddInclude("*.json")
            .AddInclude("*.ini")
            .GetResultsInFullPath(configurationDirectory);
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            var pt = fileName.Split('.');
            if (pt.Length > 2 && !env.Equals(pt[^2], StringComparison.OrdinalIgnoreCase))
                continue;
            if (fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                configuration.AddJsonFile(file, optional: true, reloadOnChange: true);
            }
            else if (fileName.EndsWith(".ini", StringComparison.OrdinalIgnoreCase))
            {
                configuration.AddIniFile(file, optional: true, reloadOnChange: true);
            }
        }
    }

    /// <summary>
    /// 配置应用
    /// </summary>
    /// <param name="app"></param>
    internal static void ConfigureApplication(IApplicationBuilder app)
    {
        ServiceProvider = app.ApplicationServices;
    }

    /// <summary>
    /// 获取配置选项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal static T GetOptions<T>()
        where T : class, new()
    {
        if (ServiceProvider != null)
        {
            var options = ServiceProvider.GetService<IOptions<T>>();
            if (options != null)
                return options.Value;
        }
        if (Configuration != null)
        {
            string sectionName = typeof(T).Name;
            if (typeof(T).GetCustomAttribute<OptionsSectionAttribute>() != null)
                sectionName = typeof(T).GetCustomAttribute<OptionsSectionAttribute>()!.Key;
            else if (typeof(T).Name.EndsWith("Options", StringComparison.OrdinalIgnoreCase))
                sectionName = typeof(T).Name[..^7];
            var options = new T();
            if (sectionName == null)
                Configuration.Bind(options);
            else
                Configuration.Bind(sectionName, options);
            if (options is IConfigureOptions<T> configureOptions)
                configureOptions.Configure(options);
            else if (options is IPostConfigureOptions<T> postConfigureOptions)
                postConfigureOptions.PostConfigure(sectionName, options);
            return options;
        }
        return new T();
    }

    /// <summary>
    /// 获取配置选项
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static object GetOptions(Type type)
    {
        if (ServiceProvider != null)
        {
            var optionsType = typeof(IOptions<>).MakeGenericType(type);
            var options = ServiceProvider.GetService(optionsType);
            if (options != null)
                return options;
        }
        if (Configuration != null)
        {
            string sectionName = type.Name;
            if (type.GetCustomAttribute<OptionsSectionAttribute>() != null)
                sectionName = type.GetCustomAttribute<OptionsSectionAttribute>()!.Key;
            else if (type.Name.EndsWith("Options", StringComparison.OrdinalIgnoreCase))
                sectionName = type.Name[..^7];
            var options = Activator.CreateInstance(type);
            if (sectionName == null)
                Configuration.Bind(options);
            else
                Configuration.Bind(sectionName, options);
            if (options != null)
                return options;
        }
        return Activator.CreateInstance(type)!;
    }

    /// <summary>
    /// 获取服务注册的生命周期类型
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    internal static ServiceLifetime? GetServiceLifetime(Type serviceType)
    {
        var serviceDescriptor = ServiceDescriptors?
            .FirstOrDefault(u => u.ServiceType == (serviceType.IsGenericType ? serviceType.GetGenericTypeDefinition() : serviceType));
        return serviceDescriptor?.Lifetime;
    }
}
