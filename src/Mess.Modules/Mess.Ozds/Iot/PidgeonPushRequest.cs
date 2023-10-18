namespace Mess.Ozds.Iot;

public record PidgeonPushRequest(
  DateTimeOffset timestamp,
  PidgeonPushRequestMeasurement[] measurements
);

public record PidgeonPushRequestMeasurement(
  string deviceId,
  DateTimeOffset timestamp,
  string data
);
