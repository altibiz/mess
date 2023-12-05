using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mess.Prelude.Json;

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
      return stringValue is not "0" and not "false"
             && (stringValue is "1" or "true"
               ? true
               : throw new JsonException("Invalid value for boolean."));
    }

    if (
      reader.TokenType is JsonTokenType.True
      or JsonTokenType.False
    )
      return reader.GetBoolean();

    if (reader.TokenType == JsonTokenType.Number)
    {
      var numberValue = reader.GetInt32();
      return numberValue != 0 && (numberValue == 1
        ? true
        : throw new JsonException("Invalid value for boolean."));
    }

    throw new JsonException("Invalid token type.");
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
