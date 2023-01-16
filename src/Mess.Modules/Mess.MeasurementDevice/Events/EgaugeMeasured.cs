using Mess.EventStore.Abstractions.Events;
using Mess.MeasurementDevice.Abstractions.Parsers.Egauge;

namespace Mess.MeasurementDevice.Events;

// TODO: better type?

public record struct EgaugeMeasured(EgaugeMeasurement Measurement) : IEvent
{
  public string Tenant => Measurement.Tenant;

  public DateTime Timestamp => Measurement.Timestamp;

  public string Source => Measurement.Source;
}
