using Mess.Ozds.Abstractions.Billing;

namespace Mess.Ozds.Abstractions.Timeseries;

public partial interface IOzdsTimeseriesQuery
{
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

  public IReadOnlyList<OzdsIotDeviceBillingData> GetBulkAbbBillingData(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public Task<IReadOnlyList<OzdsIotDeviceBillingData>> GetBulkAbbBillingDataAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public IReadOnlyList<OzdsIotDeviceBillingData> GetBulkSchneiderBillingData(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public Task<IReadOnlyList<OzdsIotDeviceBillingData>> GetBulkSchneiderBillingDataAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );
}
