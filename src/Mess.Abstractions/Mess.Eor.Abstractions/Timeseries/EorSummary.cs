namespace Mess.Eor.Abstractions.Timeseries;

public record EorSummary(
  string Tenant,
  string DeviceId,
  EorMeasurement? LastMeasurement,
  EorStatus? Status
)
{
  // TODO: proper null handling
  public static EorSummary From(
    string tenant,
    string deviceId,
    EorMeasurement? lastMeasurement,
    EorStatus? status
  )
  {
    return new EorSummary(
      Tenant: tenant,
      DeviceId: deviceId,
      LastMeasurement: lastMeasurement,
      Status: status
    );
  }
}
