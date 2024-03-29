using Mess.Eor.Abstractions.Timeseries;

namespace Mess.Eor.Iot;

public record EorPushRequest(
  DateTimeOffset Timestamp,
  float Current,
  float Voltage,
  float Temperature,
  bool HeatsinkFans,
  bool CoolingFans
)
{
  public EorMeasurement ToMeasurement(string deviceId)
  {
    return new EorMeasurement(
      deviceId,
      Timestamp,
      Current: Current,
      Voltage: Voltage,
      Temperature: Temperature,
      HeatsinkFans: HeatsinkFans,
      CoolingFans: CoolingFans
    );
  }
}
