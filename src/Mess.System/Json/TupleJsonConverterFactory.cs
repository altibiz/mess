using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mess.System.Json;

public class TupleJsonConverterFactory : JsonConverterFactory
{
  public override bool CanConvert(Type typeToConvert) =>
    typeof(ITuple).IsAssignableFrom(typeToConvert);

  public override JsonConverter CreateConverter(
    Type typeToConvert,
    JsonSerializerOptions options
  )
  {
    return (JsonConverter)
      Activator.CreateInstance(
        typeof(ValueTupleConverter<>).MakeGenericType(typeToConvert)
      )!;
  }

  private class ValueTupleConverter<T> : JsonConverter<T>
    where T : ITuple
  {
    public override void Write(
      Utf8JsonWriter writer,
      T value,
      JsonSerializerOptions options
    )
    {
      if (value is ITuple tuple)
      {
        writer.WriteStartArray();

        for (int tupleIndex = 0; tupleIndex < tuple.Length; tupleIndex++)
        {
          JsonSerializer.Serialize(writer, tuple[tupleIndex]);
        }

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
      if (!reader.Read() || reader.TokenType == JsonTokenType.StartArray)
      {
        throw new JsonException(
          $"Cannot deserialize token {reader.TokenType} to Tuple."
        );
      }

      var genericsStack = new Stack<Type[]>();
      var generics = type.GetGenericArguments();
      genericsStack.Push(generics);
      while (
        generics.Length > 7 && typeof(ITuple).IsAssignableFrom(generics[7])
      )
      {
        generics = generics[7].GetGenericArguments();
        genericsStack.Push(generics);
      }

      object? value = null;

      foreach (var chunk in genericsStack)
      {
        var tupleType = GetTupleTypeDefinition(type, chunk.Length);

        var tupleConstructorArguments = new object?[chunk.Length];

        if (chunk.Length > 0)
        {
          tupleType = tupleType.MakeGenericType(chunk);

          int arrayIndex = chunk.Length - 1;

          if (arrayIndex == 7)
          {
            tupleConstructorArguments[7] = value;
            arrayIndex--;
          }

          for (; arrayIndex >= 0; arrayIndex--)
          {
            tupleConstructorArguments[arrayIndex] = JsonSerializer.Deserialize(
              ref reader,
              chunk[arrayIndex],
              options
            );

            if (!reader.Read())
            {
              throw new JsonException(
                $"Failed reading JSON token. "
                  + $"Last read token was '{reader.TokenType}'."
              );
            }

            if (reader.TokenType == JsonTokenType.EndArray)
            {
              break;
            }
          }
        }

        value = Activator.CreateInstance(tupleType, tupleConstructorArguments);

        if (reader.TokenType == JsonTokenType.EndArray)
        {
          break;
        }
      }

      var castedValue = (T?)value;
      return castedValue
        ?? throw new JsonException("Failed deserializing Tuple.");
    }
  }

  private static Type GetTupleTypeDefinition(Type objectType, int elementCount)
  {
    return objectType.IsValueType
      ? elementCount switch
      {
        8 => typeof(ValueTuple<,,,,,,,>),
        7 => typeof(ValueTuple<,,,,,,>),
        6 => typeof(ValueTuple<,,,,,>),
        5 => typeof(ValueTuple<,,,,>),
        4 => typeof(ValueTuple<,,,>),
        3 => typeof(ValueTuple<,,>),
        2 => typeof(ValueTuple<,>),
        1 => typeof(ValueTuple<>),
        0 => typeof(ValueTuple),
        _ => throw new InvalidOperationException(),
      }
      : elementCount switch
    {
      8 => typeof(Tuple<,,,,,,,>),
      7 => typeof(Tuple<,,,,,,>),
      6 => typeof(Tuple<,,,,,>),
      5 => typeof(Tuple<,,,,>),
      4 => typeof(Tuple<,,,>),
      3 => typeof(Tuple<,,>),
      2 => typeof(Tuple<,>),
      1 => typeof(Tuple<>),
      _ => throw new InvalidOperationException(),
    };
  }
}
