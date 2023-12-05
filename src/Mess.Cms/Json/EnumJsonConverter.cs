using System.Globalization;
using Newtonsoft.Json;

namespace Mess.Cms.Json;

public class EnumJsonConverter : JsonConverter
{
  public override bool CanConvert(Type objectType)
  {
    return objectType.IsEnum;
  }

  public override object? ReadJson(
    JsonReader reader,
    Type objectType,
    object? existingValue,
    JsonSerializer serializer
  )
  {
    if (reader.TokenType == JsonToken.String)
    {
      var enumString = reader.Value as string;
      return string.IsNullOrEmpty(enumString)
        ? null
        : int.TryParse(enumString, out var number)
          ? Enum.ToObject(objectType, number)
          : Enum.Parse(objectType, enumString, true);
    }

    if (reader.TokenType == JsonToken.Integer)
    {
      var enumInt = Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture);
      return Enum.ToObject(objectType, enumInt);
    }

    throw new JsonSerializationException("Failed parsing enum");
  }

  public override void WriteJson(
    JsonWriter writer,
    object? value,
    JsonSerializer serializer
  )
  {
    writer.WriteValue(value?.ToString());
  }
}
