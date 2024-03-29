using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Mess.Prelude.Extensions.Json;

namespace Mess.Prelude.Extensions.Streams;

public static class StreamSerializationExtensions
{
  public static Stream ToJsonStream<T>(this T @this, bool pretty = true)
  {
    return new MemoryStream(
      JsonSerializer.SerializeToUtf8Bytes(
        @this,
        new JsonSerializerOptions()
          .AddMessSystemJsonOptions(pretty)
          .AddMessSystemJsonConverters()
      )
    );
  }

  public static T? FromJsonStream<T>(this Stream @this)
  {
    return JsonSerializer.Deserialize<T>(
      @this,
      new JsonSerializerOptions()
        .AddMessSystemJsonOptions()
        .AddMessSystemJsonConverters()
    );
  }

  public static object? FromJsonStream(this Stream @this, Type type)
  {
    return JsonSerializer.Deserialize(
      @this,
      type,
      new JsonSerializerOptions()
        .AddMessSystemJsonOptions()
        .AddMessSystemJsonConverters()
    );
  }

  public static async Task<T?> FromJsonStreamAsync<T>(
    this Stream @this,
    CancellationToken token = default
  )
  {
    return await JsonSerializer.DeserializeAsync<T>(
      @this,
      new JsonSerializerOptions()
        .AddMessSystemJsonOptions()
        .AddMessSystemJsonConverters(),
      token
    );
  }

  public static async Task<object?> FromJsonStreamAsync(
    this Stream @this,
    Type type,
    CancellationToken token = default
  )
  {
    return await JsonSerializer.DeserializeAsync(
      @this,
      type,
      new JsonSerializerOptions()
        .AddMessSystemJsonOptions()
        .AddMessSystemJsonConverters(),
      token
    );
  }

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
    var result = serializer.Deserialize(XmlReader.Create(@this));

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
    var result = await XDocument.LoadAsync(@this, new LoadOptions(), token);
    return result;
  }
}
