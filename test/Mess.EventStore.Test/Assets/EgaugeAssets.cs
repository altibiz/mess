using System.Xml.Linq;

namespace Mess.EventStore.Test.Assets;

public static class EgaugeAssets
{
  public static readonly XDocument Measurement =
    Mess.Util.Resources.GetXmlEmbeddedResource("assets.Egauge.xml");
}
