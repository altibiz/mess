using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mess.Prelude.Json;

public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
{
  // Days.Hours:Minutes:Seconds:Milliseconds
  public const string TimeSpanFormatString = @"d\.hh\:mm\:ss\:FFF";

  public override TimeSpan Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options
  )
  {
    return reader.TokenType != JsonTokenType.String
      ? throw new JsonException()
      : TimeSpan.TryParseExact(
        reader.GetString(),
        TimeSpanFormatString,
        null,
        out var parsedTimeSpan
      )
        ? parsedTimeSpan
        : throw new JsonException("Invalid TimeSpan format");
  }

  public override void Write(
    Utf8JsonWriter writer,
    TimeSpan value,
    JsonSerializerOptions options
  )
  {
    writer.WriteStringValue(value.ToString(TimeSpanFormatString,
      CultureInfo.InvariantCulture));
  }
}
