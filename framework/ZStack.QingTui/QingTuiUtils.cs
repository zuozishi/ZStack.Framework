using System.Text;
using System.Text.Json.Nodes;

namespace ZStack.QingTui;

/// <summary>
/// 轻推相关工具类
/// </summary>
public static class QingTuiUtils
{
    /// <summary>
    /// 验证Token是否有效
    /// </summary>
    /// <param name="token">Token</param>
    /// <param name="appid">AppId, 不为空是验证AppId一致性</param>
    /// <returns></returns>
    public static bool VerifyToken(string? token, string? appid = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;
            var pt = token.Split('.');
            if (pt.Length != 3)
                return false;
            string base64Payload = pt[1];
            int len = base64Payload.Length % 4;
            if (len > 0)
                base64Payload += new string('=', 4 - len);
            var payload = Encoding.UTF8.GetString(Convert.FromBase64String(base64Payload));
            var obj = JsonNode.Parse(payload)?.AsObject();
            if (obj == null)
                return false;
            if (appid != null && obj["appid"]?.ToString() != appid)
                return false;
            var expiresIn = (long?)obj["expires_in"];
            if (expiresIn == null || expiresIn <= DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                return false;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
