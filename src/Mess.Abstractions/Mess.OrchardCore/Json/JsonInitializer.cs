using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Mess.OrchardCore.Json;

public static class JsonInitializer
{
  [ModuleInitializer]
  public static void Initialize() =>
    JsonConvert.DefaultSettings = () =>
    {
      var settings = new JsonSerializerSettings();
      settings.Converters.Add(new TupleJsonConverter());
      settings.Converters.Add(new TimeSpanConverter());
      return settings;
    };
}
