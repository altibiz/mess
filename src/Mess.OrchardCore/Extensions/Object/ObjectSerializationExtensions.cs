using Newtonsoft.Json;
using Mess.OrchardCore.Json;
using Mess.System.Extensions.Streams;
using Mess.OrchardCore.Extensions.Streams;
using Mess.OrchardCore.Extensions.Newtonsoft;

namespace Mess.OrchardCore.Extensions.Objects;

public static class ObjectSerializationExtensions
{
  public static string ToNewtonsoftJson(
    this object? @this,
    bool pretty = true
  ) =>
    JsonConvert.SerializeObject(
      @this,
      new JsonSerializerSettings { }
        .AddMessNewtonsoftJsonSettings(pretty)
        .AddMessNewtonsoftJsonConverters()
    );

  public static T? FromNewtonsoftJson<T>(this string @this) =>
    JsonConvert.DeserializeObject<T>(
      @this,
      new JsonSerializerSettings { }
        .AddMessNewtonsoftJsonSettings()
        .AddMessNewtonsoftJsonConverters()
    );

  public static object? FromNewtonsoftJson(this string @this, Type type) =>
    JsonConvert.DeserializeObject(
      @this,
      type,
      new JsonSerializerSettings { }
        .AddMessNewtonsoftJsonSettings()
        .AddMessNewtonsoftJsonConverters()
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
