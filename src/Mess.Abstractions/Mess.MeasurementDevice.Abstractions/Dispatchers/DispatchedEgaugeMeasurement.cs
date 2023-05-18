namespace Mess.MeasurementDevice.Abstractions.Dispatchers;

public record struct DispatchedEgaugeMeasurement(
  string Source,
  string Tenant,
  DateTime Timestamp,
  float Voltage,
  float Power
);
