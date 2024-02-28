using FlexLabs.EntityFrameworkCore.Upsert;
using Mess.Ozds.Abstractions.Billing;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Prelude.Extensions.Objects;
using Mess.Timeseries.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Z.EntityFramework.Plus;

namespace Mess.Ozds.Timeseries;

public partial class OzdsTimeseriesClient
{
  public async Task<IReadOnlyList<AbbAggregate>> GetAbbQuarterHourlyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(async context =>
    {
      return await context.AbbQuarterHourlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<AbbAggregate> GetAbbQuarterHourlyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(context =>
    {
      return context.AbbQuarterHourlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<
    IReadOnlyList<SchneiderAggregate>
  > GetSchneiderQuarterHourlyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(async context =>
    {
      return await context.SchneiderQuarterHourlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<SchneiderAggregate> GetSchneiderQuarterHourlyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(context =>
    {
      return context.SchneiderQuarterHourlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<IReadOnlyList<AbbAggregate>> GetAbbDailyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(async context =>
    {
      return await context.AbbDailyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<AbbAggregate> GetAbbDailyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(context =>
    {
      return context.AbbDailyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<
    IReadOnlyList<SchneiderAggregate>
  > GetSchneiderDailyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(async context =>
    {
      return await context.SchneiderDailyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<SchneiderAggregate> GetSchneiderDailyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(context =>
    {
      return context.SchneiderDailyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<IReadOnlyList<AbbAggregate>> GetAbbMonthlyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(async context =>
    {
      return await context.AbbMonthlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<AbbAggregate> GetAbbMonthlyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(context =>
    {
      return context.AbbMonthlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<
    IReadOnlyList<SchneiderAggregate>
  > GetSchneiderMonthlyAggregateAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(async context =>
    {
      return await context.SchneiderMonthlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<SchneiderAggregate> GetSchneiderMonthlyAggregate(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(context =>
    {
      return context.SchneiderMonthlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<IReadOnlyList<AbbAggregate>> GetBulkAbbQuarterHourlyAggregateAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(async context =>
    {
      return await context.AbbQuarterHourlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<AbbAggregate> GetBulkAbbQuarterHourlyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(context =>
    {
      return context.AbbQuarterHourlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<
    IReadOnlyList<SchneiderAggregate>
  > GetBulkSchneiderQuarterHourlyAggregateAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(async context =>
    {
      return await context.SchneiderQuarterHourlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<SchneiderAggregate> GetBulkSchneiderQuarterHourlyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(context =>
    {
      return context.SchneiderQuarterHourlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<IReadOnlyList<AbbAggregate>> GetBulkAbbDailyAggregateAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(async context =>
    {
      return await context.AbbDailyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<AbbAggregate> GetBulkAbbDailyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(context =>
    {
      return context.AbbDailyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<
    IReadOnlyList<SchneiderAggregate>
  > GetBulkSchneiderDailyAggregateAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(async context =>
    {
      return await context.SchneiderDailyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<SchneiderAggregate> GetBulkSchneiderDailyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(context =>
    {
      return context.SchneiderDailyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<IReadOnlyList<AbbAggregate>> GetBulkAbbMonthlyAggregateAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(async context =>
    {
      return await context.AbbMonthlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<AbbAggregate> GetBulkAbbMonthlyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<AbbAggregate>
    >(context =>
    {
      return context.AbbMonthlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<
    IReadOnlyList<SchneiderAggregate>
  > GetBulkSchneiderMonthlyAggregateAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(async context =>
    {
      return await context.SchneiderMonthlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<SchneiderAggregate> GetBulkSchneiderMonthlyAggregate(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<SchneiderAggregate>
    >(context =>
    {
      return context.SchneiderMonthlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }
}
