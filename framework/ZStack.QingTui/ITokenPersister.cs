namespace ZStack.QingTui;

/// <summary>
/// 访问密钥持久化器
/// </summary>
public interface ITokenPersister
{
    /// <summary>
    /// 保存访问密钥
    /// </summary>
    /// <param name="appId">应用Id</param>
    /// <param name="token">访问密钥</param>
    /// <param name="expire">过期时间，秒</param>
    void SaveToken(string appId, string token, int expire);

    /// <summary>
    /// 获取访问密钥
    /// </summary>
    /// <param name="appId">应用Id</param>
    /// <returns>访问密钥</returns>
    string? GetToken(string appId);
}
