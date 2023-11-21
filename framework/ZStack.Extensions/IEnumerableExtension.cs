namespace ZStack.Extensions;

/// <summary>
/// 集合拓展方法
/// </summary>
public static class IEnumerableExtension
{
    /// <summary>
    /// 遍历集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
    {
        int index = 0;
        foreach (var item in enumerable)
        {
            action(item, index);
            index++;
        }
    }

    /// <summary>
    /// 遍历集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, int, Task> action)
    {
        int index = 0;
        foreach (var item in enumerable)
        {
            await action(item, index);
            index++;
        }
    }

    /// <summary>
    /// 集合转分隔符字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string ToString<T>(this IEnumerable<T> enumerable, char separator)
        => string.Join(separator, enumerable);

    /// <summary>
    /// 集合转分隔符字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string ToString<T>(this IEnumerable<T> enumerable, string separator)
        => string.Join(separator, enumerable);

    /// <summary>
    /// 将字典转化为QueryString格式
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="urlEncode"></param>
    /// <returns></returns>
    public static string ToQueryString(this IDictionary<string, string> dict, bool urlEncode = true)
    {
        return string.Join("&", dict.Select(p => $"{(urlEncode ? p.Key?.UrlEncode() : "")}={(urlEncode ? p.Value?.UrlEncode() : "")}"));
    }
}
