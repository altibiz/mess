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

  public Task<(decimal? First, decimal? Last, DateTimeOffset FirstDate)> GetAbbLastMonthMeasurementsAsync(
    string source
  );
  public Task<(decimal? First, decimal? Last, DateTimeOffset FirstDate)> GetSchneiderLastMonthMeasurementsAsync(
    string source
  );
  public Task<IReadOnlyList<AbbMeasurement>> GetLastAbbMeasurementsBySourcesAsync(
    List<string> sources
  );
  public Task<IReadOnlyList<SchneiderMeasurement>> GetLastSchneiderMeasurementsBySourcesAsync(
    List<string> sources
  );

  public OzdsIotDeviceBillingData GetAbbBillingData(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public Task<OzdsIotDeviceBillingData> GetAbbBillingDataAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public Task<
    IReadOnlyList<SchneiderMeasurement>
  > GetSchneiderMeasurementsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public IReadOnlyList<SchneiderMeasurement> GetSchneiderMeasurements(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public OzdsIotDeviceBillingData GetSchneiderBillingData(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public Task<OzdsIotDeviceBillingData> GetSchneiderBillingDataAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );
}
