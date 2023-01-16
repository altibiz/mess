namespace Mess.MeasurementDevice.Abstractions.Models;

public record struct EgaugeMeasurementModel(
  string Source,
  string Tenant,
  DateTime Timestamp,
  float Voltage,
  float Power
);
