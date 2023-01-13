using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Mess.System.Extensions.Object;

public static class ObjectSerializationExtensions
{
  public static string ToJson<T>(this T @this) =>
    JsonSerializer.Serialize(
      @this,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
      }
    );

  public static Stream ToJsonStream<T>(this T @this) =>
    new MemoryStream(
      JsonSerializer.SerializeToUtf8Bytes(
        @this,
        new JsonSerializerOptions
        {
          AllowTrailingCommas = true,
          WriteIndented = true,
          ReferenceHandler = ReferenceHandler.IgnoreCycles,
        }
      )
    );

  public static T? FromJson<T>(this string @this) =>
    JsonSerializer.Deserialize<T>(
      @this,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
      }
    );

  public static T? FromJsonStream<T>(this Stream @this) =>
    JsonSerializer.Deserialize<T>(
      @this,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
      }
    );

  public static ValueTask<T?> FromJsonStreamAsync<T>(
    this Stream @this,
    CancellationToken token = default
  ) =>
    JsonSerializer.DeserializeAsync<T>(
      @this,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
      },
      token
    );

  public static string ToXml<T>(this T @this)
  {
    var serializer = new XmlSerializer(typeof(T));
    string result = string.Empty;
    using var stream = new MemoryStream();
    serializer.Serialize(stream, @this);
    stream.Seek(0, SeekOrigin.Begin);
    using var reader = new StreamReader(stream);
    result = reader.ReadToEnd();
    return result;
  }

  public static Stream ToXmlStream<T>(this T @this)
  {
    var serializer = new XmlSerializer(typeof(T));
    var stream = new MemoryStream();
    serializer.Serialize(stream, @this);
    stream.Seek(0, SeekOrigin.Begin);
    return stream;
  }

  public static string ToXml(this XDocument @this)
  {
    string result = string.Empty;
    using var stream = new MemoryStream();
    @this.Save(stream);
    stream.Seek(0, SeekOrigin.Begin);
    using var reader = new StreamReader(stream);
    result = reader.ReadToEnd();
    return result;
  }

  public static Stream ToXmlStream(this XDocument @this)
  {
    var stream = new MemoryStream();
    @this.Save(stream);
    stream.Seek(0, SeekOrigin.Begin);
    return stream;
  }

  public static T? FromXml<T>(this string @this)
  {
    var serializer = new XmlSerializer(typeof(T));
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(@this));
    var result = serializer.Deserialize(stream);
    return result is not null ? (T)result : default;
  }

  public static T? FromXmlStream<T>(this Stream @this)
  {
    var serializer = new XmlSerializer(typeof(T));
    var result = serializer.Deserialize(@this);
    return result is not null ? (T)result : default;
  }

  public static XDocument? FromXml(this string @this)
  {
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(@this));
    var result = XDocument.Load(stream);
    return result;
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

  public static string ToString(this Stream stream)
  {
    using var reader = new StreamReader(stream);
    var result = reader.ReadToEnd();
    return result;
  }

  public static async ValueTask<string> ToStringAsync(
    this Stream stream,
    CancellationToken token = default
  )
  {
    using var reader = new StreamReader(stream);
#if NET6_0
    var result = await Task.Run(() => reader.ReadToEnd(), token);
#elif NET7_0
    var result = await reader.ReadToEndAsync();
#else
#error Wrong target framwork
#endif
    return result;
  }
}
