namespace Mess.MeasurementDevice.Pushing;

public record PidgeonPushRequest(
  DateTime timestamp,
  PidgeonPushRequestMeasurement[] measurements
);

public record PidgeonPushRequestMeasurement(
  string deviceId,
  DateTime timestamp,
  string data
);
