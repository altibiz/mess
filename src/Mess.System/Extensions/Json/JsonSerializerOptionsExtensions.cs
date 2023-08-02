using System.Text.Json;
using System.Text.Json.Serialization;
using Mess.System.Json;

namespace Mess.System.Extensions.Json;

public static class JsonSeriazlizerOptionsExtensions
{
  public static JsonSerializerOptions AddMessSystemJsonOptions(
    this JsonSerializerOptions options,
    bool pretty = true
  )
  {
    options.WriteIndented = pretty;
    options.AllowTrailingCommas = true;
    options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.NumberHandling = JsonNumberHandling.AllowReadingFromString;
    options.PropertyNameCaseInsensitive = true;

    return options;
  }

  public static JsonSerializerOptions AddMessSystemJsonConverters(
    this JsonSerializerOptions options
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
    new EnumJsonConverterFactory(),
    new BooleanJsonConverter(),
    new TimeSpanJsonConverter(),
    new ListJsonConverterFactory(),
    new TupleJsonConverterFactory(),
  };
}
