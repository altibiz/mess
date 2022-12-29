using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace Mess.EventStore.Parsers.Egauge;

public record class EgaugeParser(ILogger<EgaugeParser> logger) : IEgaugeParser
{
  public EgaugeMeasurement? Parse(XDocument xml)
  {
    var result = new Dictionary<string, EgaugeRegister>();
    DateTime? timestamp = null;

    try
    {
      var group = xml.Descendants().First();
      var data = group.Descendants().First();
      timestamp = DateTimeOffset
        .FromUnixTimeSeconds(
          Convert.ToInt64(
            ((string?)data.Attribute(XName.Get("time_stamp"))),
            16
          )
        )
        .UtcDateTime;
      var registers = data.Descendants(XName.Get("cname"));
      var columns = data.Descendants(XName.Get("r")).First().Descendants();

      foreach (var (register, column) in registers.Zip(columns))
      {
        var registerName = (string)register;
        var type =
          ((string?)register.Attribute(XName.Get("t")))?.ToEgaugeRegisterType()
          ?? throw new ArgumentException(
            "Register {registerName} has no type",
            registerName
          );
        var value = (decimal)column;
        result[registerName] = new(type, type.Unit(), value);
      }
    }
    catch (Exception exception)
    {
      logger.LogError(exception, "Failed parsing xml {file}", xml.BaseUri);
      return default;
    }

    return new(result, timestamp.Value);
  }
}
