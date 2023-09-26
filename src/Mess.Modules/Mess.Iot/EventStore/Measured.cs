using Mess.EventStore.Abstractions.Events;

namespace Mess.Iot.EventStore;

public record Measured(
  string Tenant,
  DateTime Timestamp,
  string DeviceId,
  string Payload
) : IEvent;
