using System.Xml.Linq;

namespace Mess.MeasurementDevice.Abstractions.Parsers.Egauge;

public interface IEgaugeParser
{
  public EgaugeMeasurement? Parse(XDocument xml);
}
