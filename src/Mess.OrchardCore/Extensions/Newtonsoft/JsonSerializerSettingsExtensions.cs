using Mess.OrchardCore.Json;
using Newtonsoft.Json;

namespace Mess.OrchardCore.Extensions.Newtonsoft;

public static class JsonSeriazlizerSettingsExtensions
{
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
    foreach (var converter in Converters)
    {
      options.Converters.Add(converter);
    }

    return options;
  }

  private static readonly JsonConverter[] Converters = new JsonConverter[]
  {
    new TupleJsonConverter(),
    new TimeSpanConverter(),
    new EnumJsonConverter(),
    new BooleanJsonConverter(),
    new ListJsonConverter(),
  };
}