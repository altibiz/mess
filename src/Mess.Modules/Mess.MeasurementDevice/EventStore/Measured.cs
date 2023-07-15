using Mess.EventStore.Abstractions.Events;

namespace Mess.MeasurementDevice.EventStore;

public record Measured(
  string Tenant,
  DateTime Timestamp,
  string ContentType,
  string DeviceId,
  string Payload
) : IEvent;
