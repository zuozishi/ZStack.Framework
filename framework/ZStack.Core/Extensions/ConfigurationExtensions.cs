using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;
using ZStack.Core.Attributes;

namespace Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
    /// <summary>
    /// 添加配置文件目录
    /// <para>支持 json、ini、y(a)ml 格式的配置文件</para>
    /// <para>通过运行时环境筛选文件(*.{EnvironmentName}.(json|ini|yaml))</para>
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="configurationDirectory"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    public static IConfigurationManager AddDirectory(this IConfigurationManager configuration, string configurationDirectory, IHostEnvironment environment)
    {
        var env = environment.EnvironmentName;
        var files = new Matcher()
            .AddInclude("*.json")
            .AddInclude("*.ini")
            .AddInclude("*.yml")
            .AddInclude("*.yaml")
            .AddExclude("*.schema.json")
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
            else if (fileName.EndsWith(".yml", StringComparison.OrdinalIgnoreCase) ||
                fileName.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase))
            {
                configuration.AddYamlFile(file, optional: true, reloadOnChange: true);
            }
        }
        return configuration;
    }

    /// <summary>
    /// 获取配置节对应的配置类实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static T GetOtions<T>(this IConfiguration configuration) where T : class, new()
    {
        string sectionName = typeof(T).Name;
        if (typeof(T).GetCustomAttribute<OptionsSectionAttribute>() != null)
            sectionName = typeof(T).GetCustomAttribute<OptionsSectionAttribute>()!.Key;
        else if (typeof(T).Name.EndsWith("Options", StringComparison.OrdinalIgnoreCase))
            sectionName = typeof(T).Name[..^7];
        var options = new T();
        if (sectionName == null)
            configuration.Bind(options);
        else
            configuration.Bind(sectionName, options);
        if (options is IConfigureOptions<T> configureOptions)
            configureOptions.Configure(options);
        else if (options is IPostConfigureOptions<T> postConfigureOptions)
            postConfigureOptions.PostConfigure(sectionName, options);
        return options;
    }
}
