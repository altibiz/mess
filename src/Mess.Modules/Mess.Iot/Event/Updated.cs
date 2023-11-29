using Mess.Event.Abstractions.Services;

namespace Mess.Iot.Event;

public record Updated(
  string Tenant,
  DateTimeOffset Timestamp,
  string DeviceId,
  string Payload
) : IEvent;
