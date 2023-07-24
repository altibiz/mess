using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using System.Xml.Linq;
using Mess.System.Extensions.Streams;
using Mess.System.Extensions.Json;

namespace Mess.System.Extensions.Objects;

public static class ObjectSerializationExtensions
{
  public static string ToJson<T>(this T @this, bool pretty = true) =>
    JsonSerializer.Serialize(
      @this,
      new JsonSerializerOptions()
        .AddMessSystemJsonOptions(pretty)
        .AddMessSystemJsonConverters()
    );

  public static string ToJson(
    this object @this,
    Type type,
    bool pretty = true
  ) =>
    JsonSerializer.Serialize(
      @this,
      type,
      new JsonSerializerOptions()
        .AddMessSystemJsonOptions(pretty)
        .AddMessSystemJsonConverters()
    );

  public static T? FromJson<T>(this string @this) =>
    JsonSerializer.Deserialize<T>(
      @this,
      new JsonSerializerOptions()
        .AddMessSystemJsonOptions()
        .AddMessSystemJsonConverters()
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

  public static T? FromXml<T>(this string @this)
  {
    var serializer = new XmlSerializer(typeof(T));
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(@this));
    var result = serializer.Deserialize(stream);
    return result is not null ? (T)result : default;
  }

  public static XDocument? FromXml(this string @this)
  {
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(@this));
    var result = XDocument.Load(stream);
    return result;
  }

  public static string GetJsonSha256Hash(this object @this)
  {
    using var stream = @this.ToJsonStream(pretty: false);
    return StreamHashExtensions.GetSha256Hash(stream);
  }

  public static string GetJsonMurMurHash(this object @this)
  {
    using var stream = @this.ToJsonStream(pretty: false);
    return StreamHashExtensions.GetMurMurHash(stream);
  }
}
