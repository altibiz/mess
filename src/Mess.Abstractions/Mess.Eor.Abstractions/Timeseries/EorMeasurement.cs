namespace Mess.Eor.Abstractions.Timeseries;

public record EorMeasurement(
  string Tenant,
  string DeviceId,
  DateTimeOffset Timestamp,
  float Voltage,
  float Current,
  float Temperature,
  bool CoolingFans,
  bool HeatsinkFans
);
