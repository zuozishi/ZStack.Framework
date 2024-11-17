using System.Security.Cryptography;

namespace System.IO;

/// <summary>
/// Stream扩展方法
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// 计算SHA1哈希值
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static string Sha1Hash(this Stream stream)
    {
        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(stream);
        return Convert.ToHexString(hash);
    }
}
