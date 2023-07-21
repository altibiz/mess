using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using System.Xml.Linq;
using Mess.System.Extensions.Streams;
using Mess.System.Json;

namespace Mess.System.Extensions.Objects;

public static class ObjectSerializationExtensions
{
  public static string ToJson<T>(this T @this, bool pretty = true) =>
    JsonSerializer.Serialize(
      @this,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        WriteIndented = pretty,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters =
        {
          new TupleJsonConverterFactory(),
          new TimeSpanConverter(),
          new EnumConverterFactory(),
          new BooleanJsonConverter(),
          new ListJsonConverterFactory()
        }
      }
    );

  public static string ToJson(
    this object @this,
    global::System.Type type,
    bool pretty = true
  ) =>
    JsonSerializer.Serialize(
      @this,
      type,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        WriteIndented = pretty,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters =
        {
          new TupleJsonConverterFactory(),
          new TimeSpanConverter(),
          new EnumConverterFactory(),
          new BooleanJsonConverter(),
          new ListJsonConverterFactory()
        }
      }
    );

  public static T? FromJson<T>(this string @this) =>
    JsonSerializer.Deserialize<T>(
      @this,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
          new TupleJsonConverterFactory(),
          new TimeSpanConverter(),
          new EnumConverterFactory(),
          new BooleanJsonConverter(),
          new ListJsonConverterFactory()
        }
      }
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
