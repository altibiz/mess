using Mess.Eor.Abstractions.Client;

namespace Mess.Eor.MeasurementDevice.Pushing;

public record EorPushRequest(
  DateTime Timestamp,
  float Current,
  float Voltage,
  float Temperature,
  bool HeatsinkFans,
  bool CoolingFans
)
{
  public EorMeasurement ToMeasurement(string tenant, string deviceId) =>
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
