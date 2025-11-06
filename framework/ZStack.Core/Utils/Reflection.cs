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
    /// 获取程序集公开类型
    /// </summary>
    /// <returns></returns>
    public static Type[] GetExportedTypes()
    {
        return [.. GetAssemblies().SelectMany(asm => {
            try
            {
                return asm.GetExportedTypes();
            }
            catch
            {
                return [];
            }
        })];
    }

    /// <summary>
    /// 加载 ZStack 程序集
    /// </summary>
    public static void LoadZStackAssemblies()
        => LoadAssemblies(["ZStack.*.dll"]);

    /// <summary>
    /// 加载程序集
    /// </summary>
    /// <param name="includePatterns"></param>
    /// <param name="excludePatterns"></param>
    /// <param name="directory"></param>
    public static void LoadAssemblies(string[] includePatterns, string[]? excludePatterns = null, string? directory = null)
    {
        if (includePatterns.Length == 0)
            return;
        var baseDir = directory ?? AppDomain.CurrentDomain.BaseDirectory;
        var mather = new Matcher(StringComparison.OrdinalIgnoreCase);
        mather.AddIncludePatterns(includePatterns);
        if (excludePatterns != null && excludePatterns.Length > 0)
            mather.AddExcludePatterns(excludePatterns);
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

    /// <summary>
    /// 获取所有程序集的类型
    /// </summary>
    /// <returns></returns>
    public static Type[] GetAssembliesTypes()
    {
        return [.. GetAssemblies().SelectMany(asm => {
            try
            {
                return asm.GetTypes();
            }
            catch
            {
                return [];
            }
        })];
    }
}