using System.Xml.Linq;

namespace Mess.EventStore.Parsers.Egauge;

public class EgaugeParser : IEgaugeParser
{
  public EgaugeMeasurement Parse(XDocument xml)
  {
    return new();
  }
}
