using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mess.System.Json;

public class BooleanJsonConverter : JsonConverter<bool>
{
  public override bool Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options
  )
  {
    if (reader.TokenType == JsonTokenType.String)
    {
      var stringValue = reader.GetString();
      if (bool.TryParse(stringValue, out bool boolValue))
      {
        return boolValue;
      }
      else
      {
        throw new JsonException("Invalid value for boolean.");
      }
    }
    else if (
      reader.TokenType == JsonTokenType.True
      || reader.TokenType == JsonTokenType.False
    )
    {
      return reader.GetBoolean();
    }
    else if (reader.TokenType == JsonTokenType.Number)
    {
      var numberValue = reader.GetInt32();
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
        throw new JsonException("Invalid value for boolean.");
      }
    }
    else
    {
      throw new JsonException("Invalid token type.");
    }
  }

  public override void Write(
    Utf8JsonWriter writer,
    bool value,
    JsonSerializerOptions options
  )
  {
    writer.WriteBooleanValue(value);
  }
}
