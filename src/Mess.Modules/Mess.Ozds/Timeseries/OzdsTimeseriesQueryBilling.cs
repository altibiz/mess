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

// TODO: use group by for bulks
// use min of active energy import total (cumulative) to get firsts
// use max of active energy import total (cumulative) to get lasts
// use max of active energy import (division of cumulative) to skip all the time stuff to get peaks

public partial class OzdsTimeseriesClient
{
  private record BulkEnergyRange(
    string Source,
    DateTimeOffset DateFrom,
    DateTimeOffset DateTo,
    decimal ActiveEnergyImportTotalMin_Wh,
    decimal ActiveEnergyImportTotalMax_Wh,
    decimal ReactiveEnergyImportTotalMin_VARh,
    decimal ReactiveEnergyImportTotalMax_VARh,
    decimal ReactiveEnergyExportTotalMin_VARh,
    decimal ReactiveEnergyExportTotalMax_VARh
  );

  private record BulkPowerPeak(
    string Source,
    DateTimeOffset Timestamp,
    decimal ActiveEnergyImportTotalMin_Wh,
    decimal ActiveEnergyImportTotalMax_Wh
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

      var peakQuery = context.AbbQuarterHourlyEnergyBounds
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Future();

      var first = firstQuery.Value;
      var last = lastQuery.Value;
      var peak = peakQuery
        .ToList()
        .OrderByDescending(measurement => measurement.ActivePowerImportAverage_W)
        .FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
          fromDate,
          toDate,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ReactiveEnergyImportTotal_VARh / 1000,
          last.ReactiveEnergyImportTotal_VARh / 1000,
          first.ReactiveEnergyExportTotal_VARh / 1000,
          last.ReactiveEnergyExportTotal_VARh / 1000,
          peak.ActivePowerImportAverage_W / 1000
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

      var peakQuery = context.AbbQuarterHourlyEnergyBounds
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Future();

      var first = await firstQuery.ValueAsync();
      var last = await lastQuery.ValueAsync();
      var peak = (await peakQuery
        .ToListAsync())
        .OrderByDescending(measurement => measurement.ActivePowerImportAverage_W)
        .FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
          fromDate,
          toDate,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ReactiveEnergyImportTotal_VARh / 1000,
          last.ReactiveEnergyImportTotal_VARh / 1000,
          first.ReactiveEnergyExportTotal_VARh / 1000,
          last.ReactiveEnergyExportTotal_VARh / 1000,
          peak.ActivePowerImportAverage_W / 1000
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

      var peakQuery = context.SchneiderQuarterHourlyEnergyBounds
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Future();

      var first = firstQuery.Value;
      var last = lastQuery.Value;
      var peak = peakQuery
        .ToList()
        .OrderByDescending(measurement => measurement.ActivePowerImportAverage_W)
        .FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
          fromDate,
          toDate,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ReactiveEnergyImportTotal_VARh / 1000,
          last.ReactiveEnergyImportTotal_VARh / 1000,
          first.ReactiveEnergyExportTotal_VARh / 1000,
          last.ReactiveEnergyExportTotal_VARh / 1000,
          peak.ActivePowerImportAverage_W / 1000
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

      var peakQuery = context.SchneiderQuarterHourlyEnergyBounds
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Future();

      var first = await firstQuery.ValueAsync();
      var last = await lastQuery.ValueAsync();
      var peak = (await peakQuery
        .ToListAsync())
        .OrderByDescending(measurement => measurement.ActivePowerImportAverage_W)
        .FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
          fromDate,
          toDate,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ActiveEnergyImportTotal_Wh / 1000,
          last.ActiveEnergyImportTotal_Wh / 1000,
          first.ReactiveEnergyImportTotal_VARh / 1000,
          last.ReactiveEnergyImportTotal_VARh / 1000,
          first.ReactiveEnergyExportTotal_VARh / 1000,
          last.ReactiveEnergyExportTotal_VARh / 1000,
          peak.ActivePowerImportAverage_W / 1000
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
          (source, measurements) => new BulkEnergyRange(
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

      var powerPeaksQuery = context.AbbQuarterHourlyEnergyBounds
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(
          measurement => measurement.Source,
          (source, measurement) => new BulkPowerPeak(
            source,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.Timestamp,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.ActiveEnergyImportTotalMin_Wh,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.ActiveEnergyImportTotalMax_Wh
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
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ReactiveEnergyImportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyImportTotalMax_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMax_VARh / 1000,
              (powerPeak.ActiveEnergyImportTotalMax_Wh - powerPeak.ActiveEnergyImportTotalMin_Wh) * 4 / 1000
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
          (source, measurements) => new BulkEnergyRange(
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

      var powerPeaksQuery = context.AbbQuarterHourlyEnergyBounds
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(
          measurement => measurement.Source,
          (source, measurement) => new BulkPowerPeak(
            source,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.Timestamp,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.ActiveEnergyImportTotalMin_Wh,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.ActiveEnergyImportTotalMax_Wh
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
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ReactiveEnergyImportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyImportTotalMax_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMax_VARh / 1000,
              (powerPeak.ActiveEnergyImportTotalMax_Wh - powerPeak.ActiveEnergyImportTotalMin_Wh) * 4 / 1000
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
          (source, measurements) => new BulkEnergyRange(
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

      var powerPeaksQuery = context.SchneiderQuarterHourlyEnergyBounds
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(
          measurement => measurement.Source,
          (source, measurement) => new BulkPowerPeak(
            source,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.Timestamp,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.ActiveEnergyImportTotalMin_Wh,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.ActiveEnergyImportTotalMax_Wh
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
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ReactiveEnergyImportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyImportTotalMax_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMax_VARh / 1000,
              (powerPeak.ActiveEnergyImportTotalMax_Wh - powerPeak.ActiveEnergyImportTotalMin_Wh) * 4 / 1000
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
          (source, measurements) => new BulkEnergyRange(
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

      var powerPeaksQuery = context.SchneiderQuarterHourlyEnergyBounds
        .Where(measurement => sources.Contains(measurement.Source))
        .Where(measurement => measurement.Timestamp >= fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(
          measurement => measurement.Source,
          (source, measurement) => new BulkPowerPeak(
            source,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.Timestamp,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.ActiveEnergyImportTotalMin_Wh,
            measurement
              .MaxBy(measurement => measurement.ActiveEnergyImportTotalMax_Wh - measurement.ActiveEnergyImportTotalMin_Wh)
              !.ActiveEnergyImportTotalMax_Wh
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
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMin_Wh / 1000,
              energyRange.ActiveEnergyImportTotalMax_Wh / 1000,
              energyRange.ReactiveEnergyImportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyImportTotalMax_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMin_VARh / 1000,
              energyRange.ReactiveEnergyExportTotalMax_VARh / 1000,
              (powerPeak.ActiveEnergyImportTotalMax_Wh - powerPeak.ActiveEnergyImportTotalMin_Wh) * 4 / 1000
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
