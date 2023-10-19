using System.Text.RegularExpressions;

namespace ZStack.Core.Utils;

/// <summary>
/// 数据验证器
/// </summary>
public static partial class DataValidator
{
    /// <summary>
    /// 验证是否为有效的手机号
    /// </summary>
    /// <param name="mobile"></param>
    /// <returns></returns>
    public static bool IsValidMobile(string mobile)
        => RegexMobile().IsMatch(mobile);

    /// <summary>
    /// 验证是否为有效的邮箱
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool IsValidEmail(string email)
        => RegexEmail().IsMatch(email);

    /// <summary>
    /// 验证是否为有效的身份证号
    /// </summary>
    /// <param name="idCard"></param>
    /// <returns></returns>
    public static bool IsValidIdCard(string idCard)
    {
        if (idCard.Length == 18)
        {
            var idCardWi = new[]
            {
                7, 9, 10, 5, 8, 4, 2, 1, 6,
                3, 7, 9, 10, 5, 8, 4, 2, 1
            };
            var idCardY = new[]
            {
                1, 0, 10, 9, 8, 7, 6, 5, 4,
                3, 2
            };
            var idCardWiSum = 0;
            for (var i = 0; i < 17; i++)
            {
                idCardWiSum += int.Parse(idCard[i].ToString()) * idCardWi[i];
            }
            var idCardMod = idCardWiSum % 11;
            var idCardLast = idCard[17];
            if (idCardMod == 2)
            {
                return idCardLast == 'X' || idCardLast == 'x';
            }
            return idCardLast == idCardY[idCardMod];
        }
        return false;
    }

    [GeneratedRegex(@"^1[3456789]\d{9}$")]
    private static partial Regex RegexMobile();

    [GeneratedRegex(@"^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+")]
    private static partial Regex RegexEmail();
}
