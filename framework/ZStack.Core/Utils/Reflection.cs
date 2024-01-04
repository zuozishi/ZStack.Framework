using Microsoft.Extensions.DependencyModel;
using System.Reflection;

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
    {
        var assemblies = new List<Assembly>();
        var dependencyContext = DependencyContext.Default;
        var libs = dependencyContext!.CompileLibraries;
        foreach (var lib in libs)
        {
            var assembly = Assembly.Load(new AssemblyName(lib.Name));
            assemblies.Add(assembly);
        }
        return assemblies;
    }

    /// <summary>
    /// 获取程序集类型
    /// </summary>
    /// <returns></returns>
    public static Type[] GetTypes()
        => GetAssemblies().SelectMany(x => x.GetTypes()).ToArray();
}
