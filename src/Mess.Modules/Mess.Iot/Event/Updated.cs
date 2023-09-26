using Mess.Event.Abstractions.Events;

namespace Mess.Iot.Event;

public record Updated(
  string Tenant,
  DateTime Timestamp,
  string DeviceId,
  string Payload
) : IEvent;
