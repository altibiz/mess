namespace Mess.Eor.Abstractions.Timeseries;

public record EorMeasurement(
  string DeviceId,
  DateTimeOffset Timestamp,
  float Voltage,
  float Current,
  float Temperature,
  bool CoolingFans,
  bool HeatsinkFans
);
