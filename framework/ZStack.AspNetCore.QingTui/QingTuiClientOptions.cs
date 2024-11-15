namespace ZStack.AspNetCore.QingTui;

/// <summary>
/// 轻推客户端配置
/// </summary>
public class QingTuiClientOptions
{
    /// <summary>
    /// 接口地址
    /// </summary>
    public string Host { get; set; } = "https://open.qingtui.com";

    /// <summary>
    /// 客户端配置
    /// </summary>
    public IReadOnlyList<ClientOptions> Clients { get; set; } = [];

    public class ClientOptions
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// 应用Id
        /// </summary>
        public string AppId { get; set; } = string.Empty;

        /// <summary>
        /// 应用密钥
        /// </summary>
        public string Secret { get; set; } = string.Empty;
    }
}
