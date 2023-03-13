using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using Mess.System.Text.Json;

namespace Mess.System.Extensions.Streams;

// TODO: optimize to stream conversions
public static class StreamSerializationExtensions
{
  public static Stream ToJsonStream<T>(this T @this, bool pretty = true) =>
    new MemoryStream(
      JsonSerializer.SerializeToUtf8Bytes(
        @this,
        new JsonSerializerOptions
        {
          AllowTrailingCommas = true,
          WriteIndented = pretty,
          ReferenceHandler = ReferenceHandler.IgnoreCycles,
          Converters = { new TupleJsonConverterFactory(), }
        }
      )
    );

  public static T? FromJsonStream<T>(this Stream @this) =>
    JsonSerializer.Deserialize<T>(
      @this,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        Converters = { new TupleJsonConverterFactory(), }
      }
    );

  public static object? FromJsonStream(
    this Stream @this,
    global::System.Type type
  ) =>
    JsonSerializer.Deserialize(
      @this,
      type,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        Converters = { new TupleJsonConverterFactory(), }
      }
    );

  public static async Task<T?> FromJsonStreamAsync<T>(
    this Stream @this,
    CancellationToken token = default
  ) =>
    await JsonSerializer.DeserializeAsync<T>(
      @this,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        Converters = { new TupleJsonConverterFactory(), }
      },
      token
    );

  public static async Task<object?> FromJsonStreamAsync(
    this Stream @this,
    global::System.Type type,
    CancellationToken token = default
  ) =>
    await JsonSerializer.DeserializeAsync(
      @this,
      type,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        Converters = { new TupleJsonConverterFactory(), }
      },
      token
    );

  public static Stream ToXmlStream<T>(this T @this)
  {
    var serializer = new XmlSerializer(typeof(T));
    var stream = new MemoryStream();
    serializer.Serialize(stream, @this);
    stream.Seek(0, SeekOrigin.Begin);
    return stream;
  }

  public static Stream ToXmlStream(this XDocument @this)
  {
    var stream = new MemoryStream();
    @this.Save(stream);
    stream.Seek(0, SeekOrigin.Begin);
    return stream;
  }

  public static T? FromXmlStream<T>(this Stream @this)
  {
    var serializer = new XmlSerializer(typeof(T));
    var result = serializer.Deserialize(@this);
    return result is not null ? (T)result : default;
  }

  public static XDocument? FromXmlStream(this Stream @this)
  {
    var result = XDocument.Load(@this);
    return result;
  }

  public static async ValueTask<XDocument?> FromXmlStreamAsync(
    this Stream @this,
    CancellationToken token = default
  )
  {
    var result = await XDocument.LoadAsync(@this, new(), token);
    return result;
  }
}
