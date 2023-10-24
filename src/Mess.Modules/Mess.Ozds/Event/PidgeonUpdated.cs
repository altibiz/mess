using Mess.Event.Abstractions.Events;

namespace Mess.Ozds.Event;

public record PidgeonUpdated(
  string Tenant,
  DateTimeOffset Timestamp,
  string DeviceId,
  string Payload
) : IEvent;
