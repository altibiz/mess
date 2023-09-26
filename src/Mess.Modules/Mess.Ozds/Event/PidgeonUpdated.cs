using Mess.Event.Abstractions.Events;

namespace Mess.Ozds.Event;

public record PidgeonUpdated(
  string Tenant,
  DateTime Timestamp,
  string DeviceId,
  string Payload
) : IEvent;
