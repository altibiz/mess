using Mess.Cms.Json;
using Newtonsoft.Json;

namespace Mess.Cms.Extensions.Newtonsoft;

public static class JsonSeriazlizerSettingsExtensions
{
  private static readonly JsonConverter[] Converters =
  {
    new TupleJsonConverter(),
    new TimeSpanJsonConverter(),
    new EnumJsonConverter(),
    new BooleanJsonConverter(),
    new ListJsonConverter()
  };

  public static JsonSerializerSettings AddMessNewtonsoftJsonSettings(
    this JsonSerializerSettings options,
    bool pretty = true
  )
  {
    options.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.Formatting = pretty ? Formatting.Indented : Formatting.None;

    return options;
  }

  public static JsonSerializerSettings AddMessNewtonsoftJsonConverters(
    this JsonSerializerSettings options
  )
  {
    foreach (var converter in Converters) options.Converters.Add(converter);

    return options;
  }
}
