using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mess.System.Json;

public class EnumJsonConverterFactory : JsonConverterFactory
{
  public override bool CanConvert(Type typeToConvert)
  {
    return typeToConvert.IsEnum;
  }

  public override JsonConverter CreateConverter(
    Type typeToConvert,
    JsonSerializerOptions options
  )
  {
    return (JsonConverter)
      Activator.CreateInstance(
        typeof(EnumJsonConverter<>).MakeGenericType(typeToConvert)
      )!;
  }

  private class EnumJsonConverter<T> : JsonConverter<T>
    where T : Enum
  {
    public override T Read(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options
    )
    {
      if (reader.TokenType == JsonTokenType.String)
      {
        string enumString = reader.GetString()!;
        if (string.IsNullOrEmpty(enumString))
        {
          return default!;
        }

        if (int.TryParse(enumString, out int number))
        {
          return (T)Enum.ToObject(typeToConvert, number);
        }
        else
        {
          return (T)Enum.Parse(typeToConvert, enumString, true);
        }
      }
      else if (reader.TokenType == JsonTokenType.Number)
      {
        int enumInt = reader.GetInt32();
        return (T)Enum.ToObject(typeToConvert, enumInt);
      }
      else
      {
        throw new JsonException("Failed parsing enum");
      }
    }

    public override void Write(
      Utf8JsonWriter writer,
      T value,
      JsonSerializerOptions options
    )
    {
      writer.WriteStringValue(value.ToString());
    }
  }
}
