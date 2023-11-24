namespace ZStack.Core.Utils;

/// <summary>
/// 数据工具类
/// </summary>
public static class DataUtils
{
    /// <summary>
    /// 对比对象是否相等
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static bool IsEqual(object? value1, object? value2)
    {
        if (value1 == null && value2 == null)
            return true;
        if (value1 == null || value2 == null)
            return false;
        if (value1 is DateTime time1 && value2 is DateTime time2)
            return time1 == time2;
        if (value1 is int int1 && value2 is int int2)
            return int1 == int2;
        if (value1 is long l1 && value2 is int l2)
            return l1 == l2;
        if (value1 is double d1 && value2 is double d2)
            return d1 == d2;
        if (value1 is decimal dec1 && value2 is decimal dec2)
            return dec1 == dec2;
        return value1.ToString() == value2.ToString();
    }
}
