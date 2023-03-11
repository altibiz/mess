using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mess.OrchardCore.Extensions.Object;

public static class ObjectSerializationExtensions
{
  public static string ToNewtonsoftJson(
    this object? @this,
    bool pretty = true
  ) =>
    JsonConvert.SerializeObject(
      @this,
      pretty ? Formatting.Indented : Formatting.None
    );

  public static Stream ToNewtonsoftJsonStream(
    this object? @this,
    bool pretty = true
  )
  {
    var serializer = JsonSerializer.Create(
      new JsonSerializerSettings
      {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = pretty ? Formatting.Indented : Formatting.None,
      }
    );

    var stream = new MemoryStream();
    using var streamWriter = new StreamWriter(stream);
    using var jsonWriter = new JsonTextWriter(streamWriter);
    serializer.Serialize(jsonWriter, @this);

    return stream;
  }

  public static T? FromNewtonsoftJson<T>(this string @this) =>
    JsonConvert.DeserializeObject<T>(@this);

  public static T? FromNewtonsoftJsonStream<T>(this Stream @this)
  {
    var serializer = JsonSerializer.Create(
      new JsonSerializerSettings
      {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
      }
    );

    using var streamWriter = new StreamReader(@this);
    using var jsonReader = new JsonTextReader(streamWriter);
    var result = serializer.Deserialize<T>(jsonReader);

    return result;
  }
}
