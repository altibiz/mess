namespace Mess.Enms.Abstractions.Timeseries;

public record EgaugeMeasurement(
  string Tenant,
  string DeviceId,
  DateTimeOffset Timestamp,
  float Voltage,
  float Power
);
