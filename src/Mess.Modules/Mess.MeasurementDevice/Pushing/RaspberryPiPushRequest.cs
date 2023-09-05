namespace Mess.MeasurementDevice.Pushing;

public record RaspberryPiPushRequest(
  RaspberryPiPushRequestMeasurement[] measurements
);

public record RaspberryPiPushRequestMeasurement(
  string deviceId,
  string request
);
