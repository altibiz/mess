using Xunit;
using Mess.EventStore.Parsers.Egauge;
using System.Xml.Linq;
using Mess.EventStore.Test.Assets;

namespace Mess.EventStore.Test;

public record class EgaugeParserTest(IEgaugeParser parser)
{
  [Theory]
  [StaticData(typeof(EgaugeAssets), nameof(EgaugeAssets.Measurement))]
  public void ParseTest(XDocument xml)
  {
    parser.Parse(xml);
  }
}
