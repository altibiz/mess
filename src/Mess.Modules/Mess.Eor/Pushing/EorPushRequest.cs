using Mess.Eor.Abstractions.Client;

namespace Mess.Eor.Pushing;

public record EorPushRequest(
  DateTimeOffset Timestamp,
  float Current,
  float Voltage,
  float Temperature,
  bool HeatsinkFans,
  bool CoolingFans
)
{
  public EorMeasurement ToMeasurement(string deviceId, string tenant) =>
    new(
      Tenant: tenant,
      DeviceId: deviceId,
      Timestamp: Timestamp,
      Current: Current,
      Voltage: Voltage,
      Temperature: Temperature,
      HeatsinkFans: HeatsinkFans,
      CoolingFans: CoolingFans
    );
}
