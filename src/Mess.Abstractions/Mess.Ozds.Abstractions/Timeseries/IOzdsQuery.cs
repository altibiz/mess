using Mess.Ozds.Abstractions.Billing;

namespace Mess.Ozds.Abstractions.Timeseries;

public interface IOzdsTimeseriesQuery
{
  public Task<IReadOnlyList<AbbMeasurement>> GetAbbMeasurementsAsync(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  );

  public IReadOnlyList<AbbMeasurement> GetAbbMeasurements(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  );

  public OzdsBillingData? GetAbbBillingData(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  );

  public Task<OzdsBillingData?> GetAbbBillingDataAsync(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  );
}
