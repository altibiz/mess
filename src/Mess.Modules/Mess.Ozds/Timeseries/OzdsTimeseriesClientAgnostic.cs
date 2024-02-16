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
  public async Task<ClosedDistributionSystemDiagnostics> GetClosedDistributionSystemDiagnostics(
    List<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      ClosedDistributionSystemDiagnostics
    >(async context =>
    {
      var firstLastQuery =
       context.FirstLastEnergiesQuery
        .FromSqlRaw(
            string.Format(
              FirstLastEnergiesQueryTemplate,
              "measurements",
              "IN ({"
                + string
                  .Join("}, { ", Enumerable
                  .Range(0, sources.Count)
                  .Select(index => index + 2))
                + "})"
            ),
            (new object[] {
              fromDate,
              toDate,
            }).Concat(sources).ToArray()
        )
        .Future();

      var peakQuery =
       context.PeakPowerQuery
        .FromSqlRaw(
            string.Format(
              PeakPowerQueryTemplate,
              "measurements",
              "IN ({"
                + string
                  .Join("}, { ", Enumerable
                  .Range(0, sources.Count)
                  .Select(index => index + 2))
                + "})"
            ),
            (new object[] {
              fromDate,
              toDate,
            }).Concat(sources).ToArray()
        )
        .Future();

      var firstLasts = await firstLastQuery.ToListAsync();
      var peaks = await peakQuery.ToListAsync();


      return new ClosedDistributionSystemDiagnostics(
        sources.Aggregate(0M, (consumption, source) =>
        {
          var ordered = firstLasts
            .Where(firstLast => firstLast.Source == source)
            .OrderBy(firstLast => firstLast.Timestamp);
          var first = ordered.FirstOrDefault();
          var last = ordered.LastOrDefault();

          return last is { } && first is { }
            ? consumption + last.ActiveEnergyImportTotal_Wh - first.ActiveEnergyImportTotal_Wh
            : consumption;
        }),
        peaks.FirstOrDefault()?.ActivePower_W ?? 0M
      );
    });
  }

  public async Task<IReadOnlyList<Measurement>> GetLastMeasurements(
    List<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      IReadOnlyList<Measurement>
    >(async context =>
    {
      var lastMeasurementsQuery =
        context.MeasurementQuery
          .FromSqlRaw(
            string.Format(
              LastMeasurementsQueryTemplate,
              "measurements",
              "IN ({"
                + string
                  .Join("}, { ", Enumerable
                  .Range(0, sources.Count)
                  .Select(index => index + 2))
                + "})"
            ),
            (new object[] {
              fromDate,
              toDate,
            }).Concat(sources).ToArray())
          .Future();

      var lastMeasurements = await lastMeasurementsQuery.ToListAsync();

      return lastMeasurements
        .Select(lastMeasurement =>
          new Measurement(
            lastMeasurement.Source,
            lastMeasurement.Timestamp,
            lastMeasurement.ActiveEnergyImportTotal_Wh,
            lastMeasurement.ActivePower_W
          )
        )
        .ToList();
    });
  }
}
