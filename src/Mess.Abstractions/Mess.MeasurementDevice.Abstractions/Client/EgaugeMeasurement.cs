namespace Mess.MeasurementDevice.Abstractions.Client;

public record EgaugeMeasurement(
  string Tenant,
  string DeviceId,
  DateTime Timestamp,
  float Voltage,
  float Power
);