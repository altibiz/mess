using Mess.Event.Abstractions.Events;

namespace Mess.Ozds.Event;

public record PidgeonMeasured(
  string Tenant,
  DateTime Timestamp,
  string DeviceId,
  string Payload
) : IEvent;
