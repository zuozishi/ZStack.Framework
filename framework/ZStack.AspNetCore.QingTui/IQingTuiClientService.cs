using ZStack.QingTui;

namespace ZStack.AspNetCore.QingTui;

/// <summary>
/// 轻推API客户端服务
/// </summary>
public interface IQingTuiClientService
{
    /// <summary>
    /// 获取API客户端
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    public QingTuiApiClient Get(string appId);

    /// <summary>
    /// 注册API客户端
    /// </summary>
    /// <param name="client"></param>
    void Add(QingTuiApiClient client);
}
