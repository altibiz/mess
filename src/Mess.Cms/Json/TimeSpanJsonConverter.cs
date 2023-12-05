using System.Globalization;
using Newtonsoft.Json;

namespace Mess.Cms.Json;

// https://stackoverflow.com/a/52504446

public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
{
  // Days.Hours:Minutes:Seconds:Milliseconds
  public const string TimeSpanFormatString = @"d\.hh\:mm\:ss\:FFF";

  public override void WriteJson(
    JsonWriter writer,
    TimeSpan value,
    JsonSerializer serializer
  )
  {
    var timespanFormatted =
      $"{value.ToString(TimeSpanFormatString, CultureInfo.InvariantCulture)}";
    writer.WriteValue(timespanFormatted);
  }

  public override TimeSpan ReadJson(
    JsonReader reader,
    Type objectType,
    TimeSpan existingValue,
    bool hasExistingValue,
    JsonSerializer serializer
  )
  {
    TimeSpan.TryParseExact(
      (string)reader.Value!,
      TimeSpanFormatString,
      null,
      out var parsedTimeSpan
    );
    return parsedTimeSpan;
  }
}
