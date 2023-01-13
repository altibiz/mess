using System.Xml.Linq;

namespace Mess.EventStore.Abstractions.Parsers.Egauge;

public interface IEgaugeParser
{
  public EgaugeMeasurement? Parse(XDocument xml);
}
