using System.Collections;
using Newtonsoft.Json;

namespace Mess.Cms.Json;

public class ListJsonConverter : JsonConverter
{
  public override bool CanConvert(Type objectType)
  {
    return typeof(IList).IsAssignableFrom(objectType)
           && (
             (
               objectType.IsGenericType
               && objectType.GetConstructor(Type.EmptyTypes) != null
             ) || objectType.IsArray
           );
  }

  public override object? ReadJson(
    JsonReader reader,
    Type objectType,
    object? existingValue,
    JsonSerializer serializer
  )
  {
    IList? result = null;
    Type? itemType = null;
    if (objectType.IsArray)
    {
      itemType = objectType.GetElementType()!;
      var listType = typeof(List<>).MakeGenericType(itemType);
      result = (IList?)Activator.CreateInstance(listType);
    }
    else
    {
      result = (IList?)Activator.CreateInstance(objectType);
      itemType = objectType.GetGenericArguments()[0];
    }

    if (result == null)
      throw new JsonSerializationException($"Cannot create type {objectType}");
    if (itemType == null)
      throw new JsonSerializationException(
        $"Cannot get item type for {objectType}"
      );

    if (reader.TokenType == JsonToken.StartArray)
    {
      while (reader.Read() && reader.TokenType != JsonToken.EndArray)
      {
        var item = serializer.Deserialize(reader, itemType);
        result.Add(item);
      }
    }
    else if (reader.TokenType == JsonToken.String)
    {
      var str = (reader.Value as string)!;
      foreach (var item in str.Split(','))
      {
        if (string.IsNullOrEmpty(item)) continue;

        var deserializedItem = JsonConvert.DeserializeObject(item, itemType);
        result.Add(deserializedItem);
      }
    }
    else
    {
      throw new JsonSerializationException("Expected a string or start array");
    }

    return objectType.IsArray
      ? typeof(Enumerable)
        .GetMethod(nameof(Enumerable.ToArray))!
        .MakeGenericMethod(itemType)
        .Invoke(null, new object[] { result })
      : result;
  }

  public override void WriteJson(
    JsonWriter writer,
    object? value,
    JsonSerializer serializer
  )
  {
    if (value is IList list)
    {
      writer.WriteStartArray();

      foreach (var item in list) serializer.Serialize(writer, item);

      writer.WriteEndArray();
    }
    else
    {
      writer.WriteNull();
    }
  }
}
