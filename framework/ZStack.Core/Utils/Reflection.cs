using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using System.Reflection;
using System.Runtime.Loader;

namespace ZStack.Core.Utils;

/// <summary>
/// 反射工具类
/// </summary>
public static class Reflection
{
    /// <summary>
    /// 获取程序集
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Assembly> GetAssemblies()
        => AppDomain.CurrentDomain.GetAssemblies();

    /// <summary>
    /// 获取程序集类型
    /// </summary>
    /// <returns></returns>
    public static Type[] GetTypes()
        => GetAssemblies().SelectMany(x => x.GetTypes()).ToArray();

    /// <summary>
    /// 加载 ZStack 程序集
    /// </summary>
    public static void LoadZStackAssemblies()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var mather = new Matcher(StringComparison.OrdinalIgnoreCase);
        mather.AddIncludePatterns(["ZStack.*.dll"]);
        var result = mather.Execute(new DirectoryInfoWrapper(new DirectoryInfo(baseDir)));
        var files = result.Files.Select(x => x.Path).ToList();
        var assemblies = GetAssemblies();
        foreach (var file in files)
        {
            var filePath = Path.Combine(baseDir, file);
            var assemblyName = AssemblyLoadContext.GetAssemblyName(filePath);
            if (assemblies.Any(x => x.FullName == assemblyName.FullName))
                continue;
            AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
        }
    }
}
