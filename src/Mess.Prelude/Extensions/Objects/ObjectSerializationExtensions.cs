using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Mess.Prelude.Extensions.Json;
using Mess.Prelude.Extensions.Streams;

namespace Mess.Prelude.Extensions.Objects;

public static class ObjectSerializationExtensions
{
  public static string ToJson<T>(this T @this, bool pretty = true)
  {
    return JsonSerializer.Serialize(
      @this,
      new JsonSerializerOptions()
        .AddMessSystemJsonOptions(pretty)
        .AddMessSystemJsonConverters()
    );
  }

  public static string ToJson(
    this object @this,
    Type type,
    bool pretty = true
  )
  {
    return JsonSerializer.Serialize(
      @this,
      type,
      new JsonSerializerOptions()
        .AddMessSystemJsonOptions(pretty)
        .AddMessSystemJsonConverters()
    );
  }

  public static T FromJson<T>(this string @this)
  {
    return JsonSerializer.Deserialize<T>(
             @this,
             new JsonSerializerOptions()
               .AddMessSystemJsonOptions()
               .AddMessSystemJsonConverters()
           )
           ?? throw new InvalidOperationException(
             $"Could not deserialize json string '{@this}' to type '{typeof(T).Name}'"
           );
  }

  public static string ToXml<T>(this T @this)
  {
    var serializer = new XmlSerializer(typeof(T));
    string result;
    using var stream = new MemoryStream();
    serializer.Serialize(stream, @this);
    stream.Seek(0, SeekOrigin.Begin);
    using var reader = new StreamReader(stream);
    result = reader.ReadToEnd();
    return result;
  }

  public static string ToXml(this XDocument @this)
  {
    string result;
    using var stream = new MemoryStream();
    @this.Save(stream);
    stream.Seek(0, SeekOrigin.Begin);
    using var reader = new StreamReader(stream);
    result = reader.ReadToEnd();
    return result;
  }

  public static T FromXml<T>(this string @this)
  {
    var serializer = new XmlSerializer(typeof(T));
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(@this));
    var result = serializer.Deserialize(XmlReader.Create(stream));
    return result is not null
      ? (T)result
      : throw new InvalidOperationException(
        $"Could not deserialize xml string '{@this}' to type '{typeof(T).Name}'"
      );
  }

  public static XDocument FromXml(this string @this)
  {
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(@this));
    var result = XDocument.Load(stream);
    return result
           ?? throw new InvalidOperationException(
             $"Could not deserialize xml string '{@this}' to type '{nameof(XDocument)}'"
           );
  }

  public static string GetJsonSha256Hash(this object @this)
  {
    using var stream = @this.ToJsonStream(false);
    return stream.GetSha256Hash();
  }

  public static string GetJsonMurMurHash(this object @this)
  {
    using var stream = @this.ToJsonStream(false);
    return stream.GetMurMurHash();
  }
}
