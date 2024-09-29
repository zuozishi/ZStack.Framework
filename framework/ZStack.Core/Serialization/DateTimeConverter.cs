using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZStack.Core.Serialization;

/// <summary>
/// DateTime序列化器
/// </summary>
/// <param name="format"></param>
public class DateTimeConverter(string format) : JsonConverter<DateTime>
{
    private readonly string _format = format;

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
         => DateTime.Parse(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format));
    }
}
