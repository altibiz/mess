using Mess.MeasurementDevice.Abstractions.Dispatchers;
using Mess.System;

namespace Mess.MeasurementDevice.Test;

public static class Assets
{
  public static readonly object[][] EgaugeMeasurements = new[]
  {
    new object[]
    {
      EmbeddedResources.GetStringEmbeddedResource(
        "Assets.Resources.Egauge.xml"
      ),
      new DispatchedEgaugeMeasurement
      {
        Source = "egauge",
        Tenant =
          "mess.measurement-device.test.egauge-measurement-dispatcher-test.dispatch Test",
        Timestamp = DateTime.FromBinary(638073944400000000),
        Voltage = 4560858953370,
        Power = 4560858953370
      },
    }
  };
}
