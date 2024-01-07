using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZStack.Core.Serialization;

/// <summary>
/// DateTime序列化器
/// </summary>
/// <param name="format"></param>
public class DateTimeConverter(string format = "yyyy-MM-dd HH:mm:ss.fff") : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(format));
    }
}

/// <summary>
/// DateTime?序列化器
/// </summary>
/// <param name="format"></param>
public class DateTimeNullableConverter(string format = "yyyy-MM-dd HH:mm:ss.fff") : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? str = reader.GetString();
        if (string.IsNullOrEmpty(str))
            return null;
        return DateTime.Parse(str);
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToString(format));
        else
            writer.WriteNullValue();
    }
}
