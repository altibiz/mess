using Newtonsoft.Json;

namespace Mess.OrchardCore.Json;

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
      if (string.IsNullOrEmpty(enumString))
      {
        return null;
      }

      if (Int32.TryParse(enumString, out int number))
      {
        return Enum.ToObject(objectType, number);
      }
      else
      {
        return Enum.Parse(objectType, enumString, true);
      }
    }
    else if (reader.TokenType == JsonToken.Integer)
    {
      var enumInt = Convert.ToInt32(reader.Value);
      return Enum.ToObject(objectType, enumInt);
    }
    else
    {
      throw new JsonSerializationException("Failed parsing enum");
    }
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
