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

    /// <summary>
    /// 获取友好文件大小字符串
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static string GetFriendlySize(this long size)
    {
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        double len = size;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}
