using System.Xml.Linq;
using Mess.System;

namespace Mess.EventStore.Test.Assets;

public static class EgaugeAssets
{
  public static readonly XDocument Measurement =
    EmbeddedResources.GetXmlEmbeddedResource("assets.Egauge.xml");
}
