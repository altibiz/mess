using System.Xml.Linq;

namespace Mess.EventStore.Parsers.Egauge;

public interface IEgaugeParser
{
  public EgaugeMeasurement Parse(XDocument xml);
}
