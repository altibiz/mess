using Mess.Ozds.Abstractions.Billing;

namespace Mess.Ozds.Abstractions.Timeseries;

public interface IOzdsTimeseriesQuery
{
  public Task<IReadOnlyList<AbbMeasurement>> GetAbbMeasurementsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public IReadOnlyList<AbbMeasurement> GetAbbMeasurements(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public OzdsBillingData? GetAbbBillingData(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public Task<OzdsBillingData?> GetAbbBillingDataAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public Task<
    IReadOnlyList<SchneiderMeasurement>
  > GetSchneiderMeasurementsAsync(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  );

  public IReadOnlyList<SchneiderMeasurement> GetSchneiderMeasurements(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  );

  public OzdsBillingData? GetSchneiderBillingData(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  );

  public Task<OzdsBillingData?> GetSchneiderBillingDataAsync(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  );
}
