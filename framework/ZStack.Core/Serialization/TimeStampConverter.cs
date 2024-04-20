using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZStack.Core.Serialization;

public class TimeStampMillisecondsConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        long? milliseconds = null;
        if (reader.TokenType == JsonTokenType.Number)
        {
            milliseconds = reader.GetInt64();
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            if (long.TryParse(reader.GetString(), out var value))
            {
                milliseconds = value;
            }
        }
        if (milliseconds == null)
            return default;
        return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(milliseconds.Value);
    }

    public override void Write(Utf8JsonWriter writer, DateTime dateTime, JsonSerializerOptions options)
    {
        var timeSpan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        var value = Convert.ToInt64(timeSpan.TotalMilliseconds);
        writer.WriteNumberValue(value);
    }
}

public class TimeStampSecondsConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        long? seconds = null;
        if (reader.TokenType == JsonTokenType.Number)
        {
            seconds = reader.GetInt64();
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            if (long.TryParse(reader.GetString(), out var value))
            {
                seconds = value;
            }
        }
        if (seconds == null)
            return default;
        return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(seconds.Value);
    }

    public override void Write(Utf8JsonWriter writer, DateTime dateTime, JsonSerializerOptions options)
    {
        var timeSpan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        var value = Convert.ToInt64(timeSpan.TotalSeconds);
        writer.WriteNumberValue(value);
    }
}
