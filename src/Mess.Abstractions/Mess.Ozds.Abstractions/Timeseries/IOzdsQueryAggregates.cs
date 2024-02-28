namespace Mess.Ozds.Abstractions.Timeseries;

public partial interface IOzdsTimeseriesQuery
{
  Task<IReadOnlyList<AbbAggregate>> GetAbbQuarterHourlyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbAggregate> GetAbbQuarterHourlyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderAggregate>
  > GetSchneiderQuarterHourlyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderAggregate> GetSchneiderQuarterHourlyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<IReadOnlyList<AbbAggregate>> GetAbbDailyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbAggregate> GetAbbDailyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderAggregate>
  > GetSchneiderDailyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderAggregate> GetSchneiderDailyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<IReadOnlyList<AbbAggregate>> GetAbbMonthlyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbAggregate> GetAbbMonthlyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderAggregate>
  > GetSchneiderMonthlyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderAggregate> GetSchneiderMonthlyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<IReadOnlyList<AbbAggregate>> GetBulkAbbQuarterHourlyAggregateAsync(
    IEnumerable<string> source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbAggregate> GetBulkAbbQuarterHourlyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderAggregate>
  > GetBulkSchneiderQuarterHourlyAggregateAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderAggregate> GetBulkSchneiderQuarterHourlyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<IReadOnlyList<AbbAggregate>> GetBulkAbbDailyAggregateAsync(
    IEnumerable<string> source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbAggregate> GetBulkAbbDailyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderAggregate>
  > GetBulkSchneiderDailyAggregateAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderAggregate> GetBulkSchneiderDailyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<IReadOnlyList<AbbAggregate>> GetBulkAbbMonthlyAggregateAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<AbbAggregate> GetBulkAbbMonthlyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  Task<
    IReadOnlyList<SchneiderAggregate>
  > GetBulkSchneiderMonthlyAggregateAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  IReadOnlyList<SchneiderAggregate> GetBulkSchneiderMonthlyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );
}
