using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using ZStack.Core.Utils;

namespace ZStack.Extensions;

/// <summary>
/// 字符串拓展方法
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// 获取MD5
    /// <see cref="MD5.GetMD5(string)"/>
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetMD5(this string str)
        => MD5.GetMD5(str);

    /// <summary>
    /// 将字符串URL编码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string UrlEncode(this string str)
    {
        return string.IsNullOrEmpty(str) ? "" : System.Web.HttpUtility.UrlEncode(str, Encoding.UTF8);
    }

    /// <summary>
    /// 将字符串URL解码
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string UrlDecode(this string str)
    {
        return string.IsNullOrEmpty(str) ? "" : System.Web.HttpUtility.UrlDecode(str, Encoding.UTF8);
    }

    #region JSON相关
    /// <summary>
    /// JSON字符串转对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T? FromJson<T>(this string json)
        => JsonSerializer.Deserialize<T>(json);

    /// <summary>
    /// JSON字符串转对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="serializerOptions"></param>
    /// <returns></returns>
    public static T? FromJson<T>(this string json, JsonSerializerOptions serializerOptions)
        => JsonSerializer.Deserialize<T>(json, serializerOptions);

    /// <summary>
    /// JSON字符串转JsonObject
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static JsonObject ToJsonObject(this string json)
        => (JsonObject)JsonNode.Parse(json)!;

    /// <summary>
    /// JSON字符串转JsonArray
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static JsonArray ToJsonArray(this string json)
        => (JsonArray)JsonNode.Parse(json)!;

    /// <summary>
    /// JSON字符串转JsonValue
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static JsonValue ToJsonValue(this string json)
        => (JsonValue)JsonNode.Parse(json)!;

    /// <summary>
    /// JSON字符串转JsonNode
    /// </summary>
    /// <param name="json"></param>
    /// <param name="jsonObject"></param>
    /// <returns></returns>
    public static bool TryToJsonObject(this string json, out JsonObject jsonObject)
    {
        var jsonNode = JsonNode.Parse(json);
        if (jsonNode is JsonObject jObject)
        {
            jsonObject = jObject;
            return true;
        }
        else
        {
            jsonObject = default!;
            return false;
        }
    }

    /// <summary>
    /// JSON字符串转JsonArray
    /// </summary>
    /// <param name="json"></param>
    /// <param name="jsonArray"></param>
    /// <returns></returns>
    public static bool TryToJsonArray(this string json, out JsonArray jsonArray)
    {
        var jsonNode = JsonNode.Parse(json);
        if (jsonNode is JsonArray jArray)
        {
            jsonArray = jArray;
            return true;
        }
        else
        {
            jsonArray = default!;
            return false;
        }
    }

    /// <summary>
    /// JSON字符串转JsonValue
    /// </summary>
    /// <param name="json"></param>
    /// <param name="jsonValue"></param>
    /// <returns></returns>
    public static bool TryToJsonValue(this string json, out JsonValue jsonValue)
    {
        var jsonNode = JsonNode.Parse(json);
        if (jsonNode is JsonValue jValue)
        {
            jsonValue = jValue;
            return true;
        }
        else
        {
            jsonValue = default!;
            return false;
        }
    }
    #endregion

    #region 验证相关
    /// <summary>
    /// 验证是否为有效的手机号
    /// <see cref="DataValidator.IsValidMobile(string)"/>
    /// </summary>
    /// <param name="mobile"></param>
    /// <returns></returns>
    public static bool IsValidMobile(this string mobile)
        => DataValidator.IsValidMobile(mobile);

    /// <summary>
    /// 验证是否为有效的邮箱
    /// <see cref="DataValidator.IsValidEmail(string)"/>
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool IsValidEmail(this string email)
        => DataValidator.IsValidEmail(email);

    /// <summary>
    /// 验证是否为有效的身份证号
    /// <see cref="DataValidator.IsValidIdCard(string)"/>
    /// </summary>
    /// <param name="idCard"></param>
    /// <returns></returns>
    public static bool IsValidIdCard(this string idCard)
        => DataValidator.IsValidIdCard(idCard);
    #endregion
}
