namespace Mess.Ozds.Iot;

public record PidgeonPushRequest(
  DateTimeOffset Timestamp,
  PidgeonPushRequestMeasurement[] Measurements
);

public record PidgeonPushRequestMeasurement(
  string DeviceId,
  DateTimeOffset Timestamp,
  string Data
);
