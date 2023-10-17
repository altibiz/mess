namespace Mess.Ozds.Pushing;

public record PidgeonPushRequest(
  DateTimeOffset timestamp,
  PidgeonPushRequestMeasurement[] measurements
);

public record PidgeonPushRequestMeasurement(
  string deviceId,
  DateTimeOffset timestamp,
  string data
);
