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
  private record BulkAggregate(
    string Source,
    DateTimeOffset DateFrom,
    DateTimeOffset DateTo,
    decimal ActiveEnergyImportTotalMin_Wh,
    decimal ActiveEnergyImportTotalMax_Wh,
    decimal ActiveEnergyImportTotalT1Min_Wh,
    decimal ActiveEnergyImportTotalT1Max_Wh,
    decimal ActiveEnergyImportTotalT2Min_Wh,
    decimal ActiveEnergyImportTotalT2Max_Wh,
    decimal ReactiveEnergyImportTotalMin_VARh,
    decimal ReactiveEnergyImportTotalMax_VARh,
    decimal ReactiveEnergyExportTotalMin_VARh,
    decimal ReactiveEnergyExportTotalMax_VARh
  );

  private record BulkPowerPeak(
    string Source,
    DateTimeOffset Timestamp,
    decimal ActiveEnergyImportTotalT1Min_Wh,
    decimal ActiveEnergyImportTotalT1Max_Wh
  );

  public OzdsIotDeviceBillingData GetAbbBillingData(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      OzdsIotDeviceBillingData
    >(context =>
    {
      var firstQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault()
        .FutureValue();

      var lastQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault()
        .FutureValue();

      var peakQuery = context.AbbQuarterHourlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Future();

      var first = firstQuery.Value;
      var last = lastQuery.Value;
      var peak = peakQuery
        .ToList()
        .OrderByDescending(measurement => measurement.ActivePowerImportT1Avg_W)
        .FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
          fromDate,
          toDate,
          first.ActiveEnergyImportTotalT1_Wh / 1000,
          last.ActiveEnergyImportTotalT1_Wh / 1000,
          first.ActiveEnergyImportTotalT2_Wh / 1000,
          last.ActiveEnergyImportTotalT2_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ReactiveEnergyImportTotal_VARh / 1000,
          last.ReactiveEnergyImportTotal_VARh / 1000,
          first.ReactiveEnergyExportTotal_VARh / 1000,
          last.ReactiveEnergyExportTotal_VARh / 1000,
          peak.ActivePowerImportT1Avg_W / 1000
        )
        : new OzdsIotDeviceBillingData(
            source,
            fromDate,
            toDate,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
          );
    });
  }

  public async Task<OzdsIotDeviceBillingData> GetAbbBillingDataAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      OzdsIotDeviceBillingData
    >(async context =>
    {
      var firstQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault()
        .FutureValue();

      var lastQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault()
        .FutureValue();

      var peakQuery = context.AbbQuarterHourlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Future();

      var first = await firstQuery.ValueAsync();
      var last = await lastQuery.ValueAsync();
      var peak = (await peakQuery
        .ToListAsync())
        .OrderByDescending(measurement => measurement.ActivePowerImportT1Avg_W)
        .FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
          fromDate,
          toDate,
          first.ActiveEnergyImportTotalT1_Wh / 1000,
          last.ActiveEnergyImportTotalT1_Wh / 1000,
          first.ActiveEnergyImportTotalT2_Wh / 1000,
          last.ActiveEnergyImportTotalT2_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ReactiveEnergyImportTotal_VARh / 1000,
          last.ReactiveEnergyImportTotal_VARh / 1000,
          first.ReactiveEnergyExportTotal_VARh / 1000,
          last.ReactiveEnergyExportTotal_VARh / 1000,
          peak.ActivePowerImportT1Avg_W / 1000
        )
        : new OzdsIotDeviceBillingData(
            source,
            fromDate,
            toDate,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
          );
    });
  }

  public OzdsIotDeviceBillingData GetSchneiderBillingData(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      OzdsIotDeviceBillingData
    >(context =>
    {
      var firstQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault()
        .FutureValue();

      var lastQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault()
        .FutureValue();

      var peakQuery = context.SchneiderQuarterHourlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Future();

      var first = firstQuery.Value;
      var last = lastQuery.Value;
      var peak = peakQuery
        .ToList()
        .OrderByDescending(measurement => measurement.ActivePowerImportT1Avg_W)
        .FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
          fromDate,
          toDate,
          first.ActiveEnergyImportTotalT1_Wh / 1000,
          last.ActiveEnergyImportTotalT1_Wh / 1000,
          first.ActiveEnergyImportTotalT2_Wh / 1000,
          last.ActiveEnergyImportTotalT2_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ReactiveEnergyImportTotal_VARh / 1000,
          last.ReactiveEnergyImportTotal_VARh / 1000,
          first.ReactiveEnergyExportTotal_VARh / 1000,
          last.ReactiveEnergyExportTotal_VARh / 1000,
          peak.ActivePowerImportT1Avg_W / 1000
        )
        : new OzdsIotDeviceBillingData(
            source,
            fromDate,
            toDate,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
          );
    });
  }

  public async Task<OzdsIotDeviceBillingData> GetSchneiderBillingDataAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      OzdsIotDeviceBillingData
    >(async context =>
    {
      var firstQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault()
        .FutureValue();

      var lastQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault()
        .FutureValue();

      var peakQuery = context.SchneiderQuarterHourlyAggregate
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Future();

      var first = await firstQuery.ValueAsync();
      var last = await lastQuery.ValueAsync();
      var peak = (await peakQuery
        .ToListAsync())
        .OrderByDescending(measurement => measurement.ActivePowerImportT1Avg_W)
        .FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
          fromDate,
          toDate,
          first.ActiveEnergyImportTotalT1_Wh / 1000,
          last.ActiveEnergyImportTotalT1_Wh / 1000,
          first.ActiveEnergyImportTotalT2_Wh / 1000,
          last.ActiveEnergyImportTotalT2_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ReactiveEnergyImportTotal_VARh / 1000,
          last.ReactiveEnergyImportTotal_VARh / 1000,
          first.ReactiveEnergyExportTotal_VARh / 1000,
          last.ReactiveEnergyExportTotal_VARh / 1000,
          peak.ActivePowerImportT1Avg_W / 1000
        )
        : new OzdsIotDeviceBillingData(
            source,
            fromDate,
            toDate,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
          );
    });
  }

  public IReadOnlyList<OzdsIotDeviceBillingData> GetBulkAbbBillingData(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      IReadOnlyList<OzdsIotDeviceBillingData>
    >(context =>
    {
      var energyRangesQuery = context.AbbMeasurements
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(measurement => sources.Contains(measurement.Source))
        .GroupBy(
          measurement => measurement.Source,
          (source, measurements) => new BulkAggregate(
            source,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.Timestamp,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.Timestamp,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotal_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotal_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT1_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT1_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT2_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT2_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyImportTotal_VARh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyImportTotal_VARh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyExportTotal_VARh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyExportTotal_VARh
          )
        )
        .Future();

      var powerPeaksQuery = context.AbbQuarterHourlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(
          measurement => measurement.Source,
          (source, measurement) => new BulkPowerPeak(
            source,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.Timestamp,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.ActiveEnergyImportTotalT1Min_Wh,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.ActiveEnergyImportTotalT1Max_Wh
          )
        )
        .Future();

      var energyRanges = energyRangesQuery
        .ToList()
        .ToDictionary(range => range.Source);
      var powerPeaks = powerPeaksQuery
        .ToList()
        .ToDictionary(peak => peak.Source);

      return sources.Select(source =>
        {
          var energyRange = energyRanges[source];
          var powerPeak = powerPeaks[source];

          return energyRange is { } && powerPeak is { }
            ? new OzdsIotDeviceBillingData(
              source,
              fromDate,
              toDate,
              energyRange.ActiveEnergyImportTotalT1Min_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT1Max_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT2Min_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT2Max_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ReactiveEnergyImportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyImportTotalMax_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMax_VARh / 1000,
              (powerPeak.ActiveEnergyImportTotalT1Max_Wh - powerPeak.ActiveEnergyImportTotalT1Min_Wh) * 4 / 1000
            )
            : new OzdsIotDeviceBillingData(
                source,
                fromDate,
                toDate,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
              );
        })
        .ToList();
    });
  }

  public async Task<IReadOnlyList<OzdsIotDeviceBillingData>> GetBulkAbbBillingDataAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      IReadOnlyList<OzdsIotDeviceBillingData>
    >(async context =>
    {
      var energyRangesQuery = context.AbbMeasurements
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(measurement => sources.Contains(measurement.Source))
        .GroupBy(
          measurement => measurement.Source,
          (source, measurements) => new BulkAggregate(
            source,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.Timestamp,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.Timestamp,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotal_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotal_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT1_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT1_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT2_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT2_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyImportTotal_VARh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyImportTotal_VARh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyExportTotal_VARh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyExportTotal_VARh
          )
        )
        .Future();

      var powerPeaksQuery = context.AbbQuarterHourlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(
          measurement => measurement.Source,
          (source, measurement) => new BulkPowerPeak(
            source,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.Timestamp,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.ActiveEnergyImportTotalT1Min_Wh,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.ActiveEnergyImportTotalT1Max_Wh
          )
        )
        .Future();

      var energyRanges = (await energyRangesQuery
        .ToListAsync())
        .ToDictionary(range => range.Source);
      var powerPeaks = (await powerPeaksQuery
        .ToListAsync())
        .ToDictionary(peak => peak.Source);

      return sources.Select(source =>
        {
          var energyRange = energyRanges[source];
          var powerPeak = powerPeaks[source];

          return energyRange is { } && powerPeak is { }
            ? new OzdsIotDeviceBillingData(
              source,
              fromDate,
              toDate,
              energyRange.ActiveEnergyImportTotalT1Min_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT1Max_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT2Min_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT2Max_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ReactiveEnergyImportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyImportTotalMax_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMax_VARh / 1000,
              (powerPeak.ActiveEnergyImportTotalT1Max_Wh - powerPeak.ActiveEnergyImportTotalT1Min_Wh) * 4 / 1000
            )
            : new OzdsIotDeviceBillingData(
                source,
                fromDate,
                toDate,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
              );
        })
        .ToList();
    });
  }

  public IReadOnlyList<OzdsIotDeviceBillingData> GetBulkSchneiderBillingData(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      IReadOnlyList<OzdsIotDeviceBillingData>
    >(context =>
    {
      var energyRangesQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(measurement => sources.Contains(measurement.Source))
        .GroupBy(
          measurement => measurement.Source,
          (source, measurements) => new BulkAggregate(
            source,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.Timestamp,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.Timestamp,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotal_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotal_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT1_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT1_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT2_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT2_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyImportTotal_VARh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyImportTotal_VARh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyExportTotal_VARh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyExportTotal_VARh
          )
        )
        .Future();

      var powerPeaksQuery = context.SchneiderQuarterHourlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(
          measurement => measurement.Source,
          (source, measurement) => new BulkPowerPeak(
            source,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.Timestamp,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.ActiveEnergyImportTotalT1Min_Wh,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.ActiveEnergyImportTotalT1Max_Wh
          )
        )
        .Future();

      var energyRanges = energyRangesQuery
        .ToList()
        .ToDictionary(range => range.Source);
      var powerPeaks = powerPeaksQuery
        .ToList()
        .ToDictionary(peak => peak.Source);

      return sources.Select(source =>
        {
          var energyRange = energyRanges[source];
          var powerPeak = powerPeaks[source];

          return energyRange is { } && powerPeak is { }
            ? new OzdsIotDeviceBillingData(
              source,
              fromDate,
              toDate,
              energyRange.ActiveEnergyImportTotalT1Min_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT1Max_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT2Min_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT2Max_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ReactiveEnergyImportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyImportTotalMax_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMax_VARh / 1000,
              (powerPeak.ActiveEnergyImportTotalT1Max_Wh - powerPeak.ActiveEnergyImportTotalT1Min_Wh) * 4 / 1000
            )
            : new OzdsIotDeviceBillingData(
                source,
                fromDate,
                toDate,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
              );
        })
        .ToList();
    });
  }

  public async Task<IReadOnlyList<OzdsIotDeviceBillingData>> GetBulkSchneiderBillingDataAsync(
    IEnumerable<string> sources,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      IReadOnlyList<OzdsIotDeviceBillingData>
    >(async context =>
    {
      var energyRangesQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(measurement => sources.Contains(measurement.Source))
        .GroupBy(
          measurement => measurement.Source,
          (source, measurements) => new BulkAggregate(
            source,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.Timestamp,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.Timestamp,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotal_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotal_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT1_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT1_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT2_Wh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ActiveEnergyImportTotalT2_Wh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyImportTotal_VARh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyImportTotal_VARh,
            measurements
              .MinBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyExportTotal_VARh,
            measurements
              .MaxBy(measurement => measurement.Timestamp)
              !.ReactiveEnergyExportTotal_VARh
          )
        )
        .Future();

      var powerPeaksQuery = context.SchneiderQuarterHourlyAggregate
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(
          measurement => measurement.Source,
          (source, measurement) => new BulkPowerPeak(
            source,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.Timestamp,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.ActiveEnergyImportTotalT1Min_Wh,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalT1Max_Wh - measurement.ActiveEnergyImportTotalT1Min_Wh)
              !.ActiveEnergyImportTotalT1Max_Wh
          )
        )
        .Future();

      var energyRanges = (await energyRangesQuery
        .ToListAsync())
        .ToDictionary(range => range.Source);
      var powerPeaks = (await powerPeaksQuery
        .ToListAsync())
        .ToDictionary(peak => peak.Source);

      return sources.Select(source =>
        {
          var energyRange = energyRanges[source];
          var powerPeak = powerPeaks[source];

          return energyRange is { } && powerPeak is { }
            ? new OzdsIotDeviceBillingData(
              source,
              fromDate,
              toDate,
              energyRange.ActiveEnergyImportTotalT1Min_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT1Max_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT2Min_Wh / 1000,
              energyRange.ActiveEnergyImportTotalT2Max_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ReactiveEnergyImportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyImportTotalMax_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMax_VARh / 1000,
              (powerPeak.ActiveEnergyImportTotalT1Max_Wh - powerPeak.ActiveEnergyImportTotalT1Min_Wh) * 4 / 1000
            )
            : new OzdsIotDeviceBillingData(
                source,
                fromDate,
                toDate,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
              );
        })
        .ToList();
    });
  }
}
