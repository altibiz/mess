namespace Mess.Eor.Abstractions.Client;

public record EorMeasurementDeviceSummary(
  string Tenant,
  string DeviceId,
  EorMeasurement? LastMeasurement,
  EorStatus? Status
)
{
  // TODO: proper null handling
  public static EorMeasurementDeviceSummary From(
    string tenant,
    string deviceId,
    EorMeasurement? lastMeasurement,
    EorStatus? status
  )
  {
    return new EorMeasurementDeviceSummary(
      Tenant: tenant,
      DeviceId: deviceId,
      LastMeasurement: lastMeasurement,
      Status: status
    );
  }
}
