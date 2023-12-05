using Newtonsoft.Json;
using Mess.Cms.Json;
using Mess.Cms.Extensions.Newtonsoft;

namespace Mess.Cms.Extensions.Streams;

public static class StreamSerializationExtensions
{
  public static Stream ToNewtonsoftJsonStream(
    this object? @this,
    bool pretty = true
  )
  {
    var serializer = JsonSerializer.Create(
      new JsonSerializerSettings { }
        .AddMessNewtonsoftJsonSettings(pretty)
        .AddMessNewtonsoftJsonConverters()
    );

    var stream = new MemoryStream();
    using var streamWriter = new StreamWriter(stream, leaveOpen: true);
    using var jsonWriter = new JsonTextWriter(streamWriter);
    serializer.Serialize(jsonWriter, @this);
    jsonWriter.Flush();
    stream.Seek(0, SeekOrigin.Begin);

    return stream;
  }

  public static T? FromNewtonsoftJsonStream<T>(this Stream @this)
  {
    var serializer = JsonSerializer.Create(
      new JsonSerializerSettings { }
        .AddMessNewtonsoftJsonSettings()
        .AddMessNewtonsoftJsonConverters()
    );

    using var streamWriter = new StreamReader(@this);
    using var jsonReader = new JsonTextReader(streamWriter);
    var result = serializer.Deserialize<T>(jsonReader);

    return result;
  }

  public static object? FromNewtonsoftJsonStream(this Stream @this, Type type)
  {
    var serializer = JsonSerializer.Create(
      new JsonSerializerSettings { }
        .AddMessNewtonsoftJsonSettings()
        .AddMessNewtonsoftJsonConverters()
    );

    using var streamWriter = new StreamReader(@this);
    using var jsonReader = new JsonTextReader(streamWriter);
    var result = serializer.Deserialize(jsonReader, type);

    return result;
  }
}
