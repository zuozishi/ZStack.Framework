using System.Reflection;
using System.Text;
using System.Text.Json;
using ZStack.Core.Attributes;
using ZStack.Extensions.Models;

namespace ZStack.Extensions;

/// <summary>
/// 对象拓展方法
/// </summary>
public static class ObjectExtension
{
    public static JsonSerializerOptions WriteIndentedJsonSerializerOptions
        => new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

    public static JsonSerializerOptions JsonSerializerOptions
        => new()
        {
            WriteIndented = false,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

    /// <summary>
    /// 对比对象属性值差异
    /// </summary>
    /// <param name="curObj">当前对象</param>
    /// <param name="newObj">新值对象</param>
    /// <param name="updateValue">是否将新值更新到原始对象</param>
    /// <returns></returns>
    public static PropertiesValueDiffList Diff(this object curObj, object newObj, bool updateValue)
    {
        var objType = curObj.GetType();
        var typeAttr = objType.GetCustomAttribute<UpdateableAttribute>();
        var typeUpdateable = typeAttr != null && !typeAttr.IsIgnore;
        var diffList = new PropertiesValueDiffList(objType);
        var properties = objType.GetProperties();
        foreach (var property in properties)
        {
            var updateAttr = property.GetCustomAttribute<UpdateableAttribute>();
            if (updateAttr == null && !typeUpdateable)
                continue;
            if (updateAttr != null && updateAttr.IsIgnore)
                continue;
            object? oldValue = property.GetValue(curObj);
            object? newValue = property.GetValue(newObj);
            if (oldValue != null && updateAttr?.IsJson == true)
            {
                oldValue = oldValue.ToJson(false);
            }
            if (newValue != null && updateAttr?.IsJson == true)
            {
                newValue = newValue.ToJson(false);
            }
            if (oldValue == null && newValue == null)
                continue;
            else if (oldValue?.ToString() != newValue?.ToString())
            {
                if (updateValue)
                    property.SetValue(curObj, newValue);
                diffList.Add(property, newValue, oldValue);
            }
        }
        return diffList;
    }

    /// <summary>
    /// 对象转JSON字符串
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="writeIndented"></param>
    /// <returns></returns>
    public static string ToJson(this object obj, bool writeIndented = true)
        => JsonSerializer.Serialize(obj, writeIndented ? WriteIndentedJsonSerializerOptions : JsonSerializerOptions);

    /// <summary>
    /// 对象转JSON字符串
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="serializerOptions"></param>
    /// <returns></returns>
    public static string ToJson(this object obj, JsonSerializerOptions serializerOptions)
        => JsonSerializer.Serialize(obj, serializerOptions);

    /// <summary>
    /// 判断类型是否实现某个泛型
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="generic">泛型类型</param>
    /// <returns>bool</returns>
    public static bool HasImplementedRawGeneric(this Type? type, Type generic)
    {
        if (type == null)
            return false;

        // 检查接口类型
        var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
        if (isTheRawGenericType) return true;

        // 检查类型
        while (type != null && type != typeof(object))
        {
            isTheRawGenericType = IsTheRawGenericType(type);
            if (isTheRawGenericType) return true;
            type = type.BaseType;
        }

        return false;

        // 判断逻辑
        bool IsTheRawGenericType(Type type) => generic == (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
    }

    /// <summary>
    /// 将字典转化为QueryString格式
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="urlEncode"></param>
    /// <returns></returns>
    public static string ToQueryString(this Dictionary<string, string> dict, bool urlEncode = true)
    {
        return string.Join("&", dict.Select(p => $"{(urlEncode ? p.Key?.UrlEncode() : "")}={(urlEncode ? p.Value?.UrlEncode() : "")}"));
    }

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
    /// 将object转换为long，若失败则返回0
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static long ParseToLong(this object obj)
    {
        try
        {
            return long.Parse(obj.ToString() ?? string.Empty);
        }
        catch
        {
            return 0L;
        }
    }

    /// <summary>
    /// 将object转换为long，若失败则返回指定值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static long ParseToLong(this string str, long defaultValue)
    {
        try
        {
            return long.Parse(str);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// 将object转换为double，若失败则返回0
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static double ParseToDouble(this object obj)
    {
        try
        {
            return double.Parse(obj.ToString() ?? string.Empty);
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// 将object转换为double，若失败则返回指定值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static double ParseToDouble(this object str, double defaultValue)
    {
        try
        {
            return double.Parse(str.ToString() ?? string.Empty);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// 将string转换为DateTime，若失败则返回日期最小值
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static DateTime ParseToDateTime(this string str)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return DateTime.MinValue;
            }
            if (str.Contains('-') || str.Contains('/'))
            {
                return DateTime.Parse(str);
            }
            else
            {
                int length = str.Length;
                return length switch
                {
                    4 => DateTime.ParseExact(str, "yyyy", System.Globalization.CultureInfo.CurrentCulture),
                    6 => DateTime.ParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture),
                    8 => DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                    10 => DateTime.ParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture),
                    12 => DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture),
                    14 => DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                    _ => DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                };
            }
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    /// <summary>
    /// 将string转换为DateTime，若失败则返回默认值
    /// </summary>
    /// <param name="str"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static DateTime ParseToDateTime(this string str, DateTime? defaultValue)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return defaultValue.GetValueOrDefault();
            }
            if (str.Contains('-') || str.Contains('/'))
            {
                return DateTime.Parse(str);
            }
            else
            {
                int length = str.Length;
                return length switch
                {
                    4 => DateTime.ParseExact(str, "yyyy", System.Globalization.CultureInfo.CurrentCulture),
                    6 => DateTime.ParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture),
                    8 => DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                    10 => DateTime.ParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture),
                    12 => DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture),
                    14 => DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                    _ => DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                };
            }
        }
        catch
        {
            return defaultValue.GetValueOrDefault();
        }
    }

    /// <summary>
    /// 判断是否有值
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this object? obj)
    {
        return obj == null || string.IsNullOrEmpty(obj.ToString());
    }
}
