namespace Mess.Ozds.Abstractions.Timeseries;

public partial interface IOzdsTimeseriesQuery
{
  Task<IReadOnlyList<AbbEnergyRange>> GetAbbQuarterHourlyEnergyRangeAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbEnergyRange> GetAbbQuarterHourlyEnergyRange(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderEnergyRange>
  > GetSchneiderQuarterHourlyEnergyRangeAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderEnergyRange> GetSchneiderQuarterHourlyEnergyRange(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<IReadOnlyList<AbbEnergyRange>> GetAbbMonthlyEnergyRangeAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbEnergyRange> GetAbbMonthlyEnergyRange(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderEnergyRange>
  > GetSchneiderMonthlyEnergyRangeAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderEnergyRange> GetSchneiderMonthlyEnergyRange(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<IReadOnlyList<AbbEnergyRange>> GetBulkAbbQuarterHourlyEnergyRangeAsync(
    IEnumerable<string> source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbEnergyRange> GetBulkAbbQuarterHourlyEnergyRange(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderEnergyRange>
  > GetBulkSchneiderQuarterHourlyEnergyRangeAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderEnergyRange> GetBulkSchneiderQuarterHourlyEnergyRange(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<IReadOnlyList<AbbEnergyRange>> GetBulkAbbMonthlyEnergyRangeAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbEnergyRange> GetBulkAbbMonthlyEnergyRange(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderEnergyRange>
  > GetBulkSchneiderMonthlyEnergyRangeAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderEnergyRange> GetBulkSchneiderMonthlyEnergyRange(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );
}
