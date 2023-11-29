using Mess.Event.Abstractions.Services;

namespace Mess.Iot.Event;

public record Measured(
  string Tenant,
  DateTimeOffset Timestamp,
  string DeviceId,
  string Payload
) : IEvent;
