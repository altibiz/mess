using Mess.EventStore.Abstractions.Events;

namespace Mess.MeasurementDevice.EventStore;

public record Measured(
  string ArrivalTenant,
  DateTime ArrivalTimestamp,
  string DispatcherId,
  string Measurement
) : IEvent
{
  public string Tenant => ArrivalTenant;

  public DateTime Timestamp => ArrivalTimestamp;
}
