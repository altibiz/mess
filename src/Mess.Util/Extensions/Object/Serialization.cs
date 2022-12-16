using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mess.Util.Extensions.Object;

public static class ObjectSerializationExtensions
{
  public static string ToJson<T>(this T @this) =>
    JsonSerializer.Serialize(
      @this,
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
      }
    );

  public static T ConsoleLog<T>(this T @this)
  {
    var json = @this.ToJson();
    Console.WriteLine(json);
    return @this;
  }
}
