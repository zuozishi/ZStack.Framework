namespace System;

public static class NumberExtensions
{
    /// <summary>
    /// Unix时间戳转为DateTime（秒）
    /// </summary>
    /// <param name="unixTime"></param>
    /// <returns></returns>
    public static DateTime ToDateTimeSeconds(this long unixTime)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
    }

    /// <summary>
    /// Unix时间戳转为DateTime（毫秒）
    /// </summary>
    /// <param name="unixTime"></param>
    /// <returns></returns>
    public static DateTime ToDateTimeMiMilliseconds(this long unixTime)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime;
    }
}
