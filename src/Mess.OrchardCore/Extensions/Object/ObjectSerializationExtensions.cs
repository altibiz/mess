using Newtonsoft.Json;
using Mess.OrchardCore.Json;
using Mess.System.Extensions.Streams;
using Mess.OrchardCore.Extensions.Streams;

namespace Mess.OrchardCore.Extensions.Objects;

public static class ObjectSerializationExtensions
{
  public static string ToNewtonsoftJson(
    this object? @this,
    bool pretty = true
  ) =>
    JsonConvert.SerializeObject(
      @this,
      new JsonSerializerSettings
      {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = pretty ? Formatting.Indented : Formatting.None,
        Converters = new JsonConverter[]
        {
          new TupleJsonConverter(),
          new TimeSpanConverter()
        }
      }
    );

  public static T? FromNewtonsoftJson<T>(this string @this) =>
    JsonConvert.DeserializeObject<T>(
      @this,
      new JsonSerializerSettings
      {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Converters = new JsonConverter[]
        {
          new TupleJsonConverter(),
          new TimeSpanConverter()
        }
      }
    );

  public static object? FromNewtonsoftJson(this string @this, Type type) =>
    JsonConvert.DeserializeObject(
      @this,
      type,
      new JsonSerializerSettings
      {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Converters = new JsonConverter[]
        {
          new TupleJsonConverter(),
          new TimeSpanConverter()
        }
      }
    );

  public static string GetNewtonsoftJsonSha256Hash(this object @this)
  {
    using var stream = @this.ToNewtonsoftJsonStream(pretty: false);
    return StreamHashExtensions.GetSha256Hash(stream);
  }

  public static string GetNewtonsoftJsonMurMurHash(this object @this)
  {
    using var stream = @this.ToNewtonsoftJsonStream(pretty: false);
    return StreamHashExtensions.GetMurMurHash(stream);
  }
}
