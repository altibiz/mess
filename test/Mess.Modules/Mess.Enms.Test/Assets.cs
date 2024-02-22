using Mess.Enms.Abstractions.Timeseries;
using Mess.Prelude;

namespace Mess.Enms.Test;

public static class Assets
{
  public static readonly object[][] EgaugeMeasurements =
  {
    new object[]
    {
      EmbeddedResources.GetStringEmbeddedResource(
        "Assets.Resources.Egauge.xml"
      ),
      new EgaugeMeasurement(
        DeviceId: "egauge",
        Timestamp: new DateTimeOffset(DateTime.FromBinary(638073944400000000)),
        Voltage: 4560858953370,
        Power: 27848558872
      )
    }
  };
}
