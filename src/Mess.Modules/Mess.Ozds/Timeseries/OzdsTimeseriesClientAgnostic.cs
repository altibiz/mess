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
    if (sources.Count == 0)
    {
      return new ClosedDistributionSystemDiagnostics(0M, 0M);
    }

    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      ClosedDistributionSystemDiagnostics
    >(async context =>
    {
      var firstLastInput = string.Format(
              FirstLastEnergiesQueryTemplate,
              "= {"
                + string
                  .Join("} or \"Source\" = {", Enumerable
                  .Range(0, sources.Count)
                  .Select(index => index + 2))
                + "}"
            );


      var firstLastQuery =
       context.FirstLastEnergiesQuery
        .FromSqlRaw(
            firstLastInput,
            (new object[] {
              fromDate,
              toDate,
            }).Concat(sources).ToArray()
        )
        .Future();

      var peakInput = string.Format(
              PeakPowerQueryTemplate,
              "= {"
                + string
                  .Join("} or \"Source\" = {", Enumerable
                  .Range(0, sources.Count)
                  .Select(index => index + 2))
                + "}"
            );

      var peakQuery =
       context.IntervalAveragePowerQuery
        .FromSqlRaw(
            peakInput,
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
}
