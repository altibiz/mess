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
  public async Task<(decimal? First, decimal? Last, DateTimeOffset FirstDate)> GetAbbLastMonthMeasurementsAsync(
    string source,
    DateTimeOffset startDate,
    DateTimeOffset endDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      (decimal?, decimal?, DateTimeOffset)
    >(async context =>
    {
      AbbMeasurement? first = null;
      var query = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > startDate)
        .Where(measurement => measurement.Timestamp < endDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel());
      var last = await query
        .FirstOrDefaultAsync();

      if (last != null)
      {
        first = await context.AbbMeasurements
          .Where(measurement => measurement.Source == source)
          .Where(measurement => measurement.Timestamp > startDate)
          .Where(measurement => measurement.Timestamp < endDate)
          .OrderByDescending(measurement => measurement.Timestamp)
          .Select(measurement => measurement.ToModel())
          .FirstOrDefaultAsync();
        if (first == null)
          return (0, last.ActiveEnergyImportTotal_Wh, DateTime.UtcNow);
      }
      else
      {
        return (0, 0, DateTime.UtcNow);
      }

      return (first.ActiveEnergyImportTotal_Wh, last.ActiveEnergyImportTotal_Wh, first.Timestamp);
    });
  }

  public async Task<(decimal? First, decimal? Last, DateTimeOffset FirstDate)> GetSchneiderLastMonthMeasurementsAsync(
      string source,
    DateTimeOffset startDate,
    DateTimeOffset endDate
    )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      (decimal?, decimal?, DateTimeOffset)
    >(async context =>
    {
      SchneiderMeasurement? first = null;
      var query = context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > startDate)
        .Where(measurement => measurement.Timestamp < endDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel());
      var last = await query
        .FirstOrDefaultAsync();

      if (last != null)
      {
        first = await context.SchneiderMeasurements
          .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > startDate)
        .Where(measurement => measurement.Timestamp < endDate)
          .OrderByDescending(measurement => measurement.Timestamp)
          .Select(measurement => measurement.ToModel())
          .FirstOrDefaultAsync();
        if (first == null)
          return (0, last.ActiveEnergyImportTotal_Wh, DateTime.UtcNow);
      }
      else
        return (0, 0, DateTime.UtcNow);

      return (first.ActiveEnergyImportTotal_Wh, last.ActiveEnergyImportTotal_Wh, first.Timestamp);
    });
  }

  public async Task<IReadOnlyList<AbbMeasurement>> GetLastAbbMeasurementsBySourcesAsync(
  List<string> sources
)
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      IReadOnlyList<AbbMeasurement>
    >(async context =>
    {
      return await context.AbbMeasurements
        .Where(measurement => sources.Contains(measurement.Source))
        .GroupBy(measurement => measurement.Source)
        .Select(group => group.OrderByDescending(measurement => measurement.Timestamp).First().ToModel())
        .ToListAsync();
    });
  }

  public async Task<IReadOnlyList<SchneiderMeasurement>> GetLastSchneiderMeasurementsBySourcesAsync(
List<string> sources
)
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      IReadOnlyList<SchneiderMeasurement>
    >(async context =>
    {
      return await context.SchneiderMeasurements
        .Where(measurement => sources.Contains(measurement.Source))
        .GroupBy(measurement => measurement.Source)
        .Select(group => group.OrderByDescending(measurement => measurement.Timestamp).First().ToModel())
        .ToListAsync();
    });
  }
}
