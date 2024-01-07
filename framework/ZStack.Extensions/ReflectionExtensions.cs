using System.Reflection;
using System.Runtime.CompilerServices;

namespace ZStack.Extensions;

public static class ReflectionExtensions
{
    /// <summary>
    /// 获取方法真实返回类型
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public static Type GetRealReturnType(this MethodInfo method)
    {
        // 判断是否是异步方法
        var isAsyncMethod = method.IsAsync();

        // 获取类型返回值并处理 Task 和 Task<T> 类型返回值
        var returnType = method.ReturnType;
        return isAsyncMethod ? (returnType.GenericTypeArguments.FirstOrDefault() ?? typeof(void)) : returnType;
    }

    /// <summary>
    /// 判断方法是否是异步
    /// </summary>
    /// <param name="method">方法</param>
    /// <returns></returns>
    public static bool IsAsync(this MethodInfo method)
    {
        return method.GetCustomAttribute<AsyncMethodBuilderAttribute>() != null
            || method.ReturnType.ToString().StartsWith(typeof(Task).FullName!);
    }
}
