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
}
