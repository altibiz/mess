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
      if (stringValue == "0" || stringValue == "false")
      {
        return false;
      }
      else if (stringValue == "1" || stringValue == "true")
      {
        return true;
      }
      else
      {
        throw new JsonSerializationException("Invalid value for boolean.");
      }
    }
    else if (reader.TokenType == JsonToken.Boolean)
    {
      return (bool)reader.Value;
    }
    else if (reader.TokenType == JsonToken.Integer)
    {
      var numberValue = Convert.ToInt32(reader.Value);
      if (numberValue == 0)
      {
        return false;
      }
      else if (numberValue == 1)
      {
        return true;
      }
      else
      {
        throw new JsonSerializationException("Invalid value for boolean.");
      }
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
