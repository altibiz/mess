namespace Mess.Eor.Abstractions.Client;

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
