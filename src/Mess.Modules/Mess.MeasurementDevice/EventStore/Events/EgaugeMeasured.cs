using Mess.EventStore.Abstractions.Events;
using Mess.MeasurementDevice.Abstractions.Models;

namespace Mess.MeasurementDevice.EventStore.Events;

// TODO: better type?

public record struct EgaugeMeasured(EgaugeMeasurementModel Model) : IEvent
{
  public string Tenant => Model.Tenant;

  public DateTime Timestamp => Model.Timestamp;

  public string Source => Model.Source;
}
