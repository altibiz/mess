namespace Mess.Enms.Abstractions.Timeseries;

public record EgaugeMeasurement(
  string DeviceId,
  DateTimeOffset Timestamp,
  float Voltage,
  float Power
);
