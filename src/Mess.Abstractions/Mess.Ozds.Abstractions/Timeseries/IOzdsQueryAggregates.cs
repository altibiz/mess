namespace Mess.Ozds.Abstractions.Timeseries;

public partial interface IOzdsTimeseriesQuery
{
  Task<IReadOnlyList<AbbEnergyBounds>> GetAbbQuarterHourlyEnergyBoundsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbEnergyBounds> GetAbbQuarterHourlyEnergyBounds(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderEnergyBounds>
  > GetSchneiderQuarterHourlyEnergyBoundsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderEnergyBounds> GetSchneiderQuarterHourlyEnergyBounds(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<IReadOnlyList<AbbEnergyBounds>> GetAbbMonthlyEnergyBoundsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbEnergyBounds> GetAbbMonthlyEnergyBounds(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderEnergyBounds>
  > GetSchneiderMonthlyEnergyBoundsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderEnergyBounds> GetSchneiderMonthlyEnergyBounds(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<IReadOnlyList<AbbEnergyBounds>> GetBulkAbbQuarterHourlyEnergyBoundsAsync(
    IEnumerable<string> source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbEnergyBounds> GetBulkAbbQuarterHourlyEnergyBounds(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderEnergyBounds>
  > GetBulkSchneiderQuarterHourlyEnergyBoundsAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderEnergyBounds> GetBulkSchneiderQuarterHourlyEnergyBounds(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<IReadOnlyList<AbbEnergyBounds>> GetBulkAbbMonthlyEnergyBoundsAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbEnergyBounds> GetBulkAbbMonthlyEnergyBounds(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderEnergyBounds>
  > GetBulkSchneiderMonthlyEnergyBoundsAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderEnergyBounds> GetBulkSchneiderMonthlyEnergyBounds(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );
}
