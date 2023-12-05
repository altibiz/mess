using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mess.Prelude.Json;

public class ListJsonConverterFactory : JsonConverterFactory
{
  public override bool CanConvert(Type typeToConvert)
  {
    return typeof(IList).IsAssignableFrom(typeToConvert)
           && (
             (
               typeToConvert.IsGenericType
               && typeToConvert.GetConstructor(Type.EmptyTypes) is not null
             ) || typeToConvert.IsArray
           );
  }

  public override JsonConverter CreateConverter(
    Type typeToConvert,
    JsonSerializerOptions options
  )
  {
    return (JsonConverter)
      Activator.CreateInstance(
        typeof(ListJsonConverter<>).MakeGenericType(typeToConvert)
      )!;
  }

  private class ListJsonConverter<T> : JsonConverter<T>
    where T : IList
  {
    public override void Write(
      Utf8JsonWriter writer,
      T value,
      JsonSerializerOptions options
    )
    {
      if (value is IList list)
      {
        writer.WriteStartArray();

        foreach (var item in list)
          JsonSerializer.Serialize(writer, item, options);

        writer.WriteEndArray();
      }
      else
      {
        writer.WriteNullValue();
      }
    }

    public override T Read(
      ref Utf8JsonReader reader,
      Type type,
      JsonSerializerOptions options
    )
    {
      IList? result = null;
      Type? itemType = null;
      if (type.IsArray)
      {
        itemType = type.GetElementType()!;
        var listType = typeof(List<>).MakeGenericType(itemType);
        result = (IList?)Activator.CreateInstance(listType);
      }
      else
      {
        result = (IList?)Activator.CreateInstance(type);
        itemType = type.GetGenericArguments()[0];
      }

      if (result is null) throw new JsonException($"Cannot create type {type}");
      if (itemType is null)
        throw new JsonException($"Cannot get item type for {type}");

      if (reader.TokenType == JsonTokenType.StartArray)
      {
        reader.Read();

        while (reader.TokenType != JsonTokenType.EndArray)
        {
          var item = JsonSerializer.Deserialize(ref reader, itemType, options);
          result.Add(item);
          reader.Read();
        }
      }
      else if (
        reader.TokenType == JsonTokenType.String
        && reader.GetString() is string str
      )
      {
        foreach (var item in str.Split(','))
        {
          if (string.IsNullOrEmpty(item)) continue;

          var deserializedItem = JsonSerializer.Deserialize(
            item,
            itemType,
            options
          );

          result.Add(deserializedItem);
        }
      }
      else
      {
        throw new JsonException("Expected a string or start array");
      }

      return type.IsArray
        ? (T)
        typeof(Enumerable)
          .GetMethod(nameof(Enumerable.ToArray))!
          .MakeGenericMethod(itemType)
          .Invoke(null, new object[] { result })!
        : (T)result;
    }
  }
}
