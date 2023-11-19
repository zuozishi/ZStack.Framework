using System.Text;

namespace ZStack.Core.Utils;

/// <summary>
/// MD5工具类
/// </summary>
public static class MD5
{
    /// <summary>
    /// 获取MD5
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetMD5(string str)
    {
        var inputBytes = Encoding.UTF8.GetBytes(str);
        return GetMD5(inputBytes);
    }

    /// <summary>
    /// 获取MD5
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string GetMD5(byte[] bytes)
    {
        var hashBytes = System.Security.Cryptography.MD5.HashData(bytes);
        var sb = new StringBuilder();
        foreach (var t in hashBytes)
        {
            sb.Append(t.ToString("X2"));
        }
        return sb.ToString();
    }

    /// <summary>
    /// 获取MD5
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static string GetMD5(Stream stream)
    {
        stream.Position = 0;
        var hashBytes = System.Security.Cryptography.MD5.HashData(stream);
        var sb = new StringBuilder();
        foreach (var t in hashBytes)
        {
            sb.Append(t.ToString("X2"));
        }
        return sb.ToString();
    }
}
