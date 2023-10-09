namespace Mess.Iot.Abstractions.Client;

public record EgaugeMeasurement(
  string Tenant,
  string DeviceId,
  DateTimeOffset Timestamp,
  float Voltage,
  float Power
);
