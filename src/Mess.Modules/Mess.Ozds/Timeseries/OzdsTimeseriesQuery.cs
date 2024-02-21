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
  public IReadOnlyList<AbbMeasurement> GetAbbMeasurements(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<AbbMeasurement>
    >(context =>
    {
      return context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<IReadOnlyList<AbbMeasurement>> GetAbbMeasurementsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<AbbMeasurement>
    >(async context =>
    {
      return await context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<SchneiderMeasurement> GetSchneiderMeasurements(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<SchneiderMeasurement>
    >(context =>
    {
      return context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<
    IReadOnlyList<SchneiderMeasurement>
  > GetSchneiderMeasurementsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<SchneiderMeasurement>
    >(async context =>
    {
      return await context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<AbbMeasurement> GetBulkAbbMeasurements(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<AbbMeasurement>
    >(context =>
    {
      return context.AbbMeasurements
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<IReadOnlyList<AbbMeasurement>> GetBulkAbbMeasurementsAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<AbbMeasurement>
    >(async context =>
    {
      return await context.AbbMeasurements
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<SchneiderMeasurement> GetBulkSchneiderMeasurements(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<SchneiderMeasurement>
    >(context =>
    {
      return context.SchneiderMeasurements
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<
    IReadOnlyList<SchneiderMeasurement>
  > GetBulkSchneiderMeasurementsAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<SchneiderMeasurement>
    >(async context =>
    {
      return await context.SchneiderMeasurements
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }
}
