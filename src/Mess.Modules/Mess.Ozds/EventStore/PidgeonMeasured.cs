using Mess.EventStore.Abstractions.Events;

namespace Mess.Ozds.EventStore;

public record PidgeonMeasured(
  string Tenant,
  DateTime Timestamp,
  string DeviceId,
  string Payload
) : IEvent;