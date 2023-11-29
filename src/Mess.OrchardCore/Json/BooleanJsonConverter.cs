using System.Globalization;
using Newtonsoft.Json;

namespace Mess.OrchardCore.Json;

public class BooleanJsonConverter : JsonConverter<bool>
{
  public override bool ReadJson(
    JsonReader reader,
    Type objectType,
    bool existingValue,
    bool hasExistingValue,
    JsonSerializer serializer
  )
  {
    if (
      reader.TokenType == JsonToken.Null
      || reader.TokenType == JsonToken.None
      || reader.Value == null
    )
    {
      return false;
    }

    if (reader.TokenType == JsonToken.String)
    {
      var stringValue = reader.Value.ToString();
      return stringValue is not "0" and not "false"
&& (stringValue is "1" or "true" ? true : throw new JsonSerializationException("Invalid value for boolean."));
    }
    else if (reader.TokenType == JsonToken.Boolean)
    {
      return (bool)reader.Value;
    }
    else if (reader.TokenType == JsonToken.Integer)
    {
      var numberValue = Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture);
      return numberValue != 0 && (numberValue == 1 ? true : throw new JsonSerializationException("Invalid value for boolean."));
    }
    else
    {
      throw new JsonSerializationException("Invalid token type.");
    }
  }

  public override void WriteJson(
    JsonWriter writer,
    bool value,
    JsonSerializer serializer
  )
  {
    writer.WriteValue((bool?)value);
  }
}
