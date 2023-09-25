using Mess.EventStore.Abstractions.Events;

namespace Mess.Ozds.EventStore;

public record PidgeonUpdated(
  string Tenant,
  DateTime Timestamp,
  string DeviceId,
  string Payload
) : IEvent;
