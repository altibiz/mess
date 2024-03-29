using System.Text.Json;
using System.Text.Json.Serialization;
using Mess.Prelude.Json;

namespace Mess.Prelude.Extensions.Json;

public static class JsonSeriazlizerOptionsExtensions
{
  private static readonly JsonConverter[] Converters =
  {
    new EnumJsonConverterFactory(),
    new BooleanJsonConverter(),
    new TimeSpanJsonConverter(),
    new ListJsonConverterFactory(),
    new TupleJsonConverterFactory()
  };

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
    foreach (var converter in Converters) options.Converters.Add(converter);

    return options;
  }
}
