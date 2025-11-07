﻿using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.Configuration;

public static class ConfigurationManagerExtensions
{
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
}