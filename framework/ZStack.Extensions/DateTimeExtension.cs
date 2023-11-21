namespace ZStack.Extensions;

/// <summary>
/// DateTime扩展方法
/// </summary>
public static class DateTimeExtension
{
    /// <summary>
    /// 获取时间戳（秒）
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ToTimestampSeconds(this DateTime dateTime)
    {
        var timeSpan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(timeSpan.TotalSeconds);
    }

    /// <summary>
    /// 获取时间戳（毫秒）
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ToTimestampMilliseconds(this DateTime dateTime)
    {
        var timeSpan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(timeSpan.TotalMilliseconds);
    }
}
