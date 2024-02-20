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
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault()
        .FutureValue();

      var lastQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault()
        .FutureValue();

      var peakQuery =
       context.QuarterHourAveragePower
        .FromSqlRaw(
            string.Format(
              PeakPowerQueryTemplate,
              string.Format(
                AbbBaseSubquery,
                "= {2}"
              )
            ),
            fromDate,
            toDate,
            source
        )
        .Future();

      var first = firstQuery.Value;
      var last = lastQuery.Value;
      var peak = peakQuery.ToList().FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
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
          peak.ActivePower_W / 1000
        )
        : new OzdsIotDeviceBillingData(
            source,
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
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault()
        .FutureValue();

      var lastQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault()
        .FutureValue();

      var peakQuery =
       context.QuarterHourAveragePower
        .FromSqlRaw(
            string.Format(
              PeakPowerQueryTemplate,
              string.Format(
                AbbBaseSubquery,
                "= {2}"
              )
            ),
            fromDate,
            toDate,
            source
        )
        .Future();

      var first = await firstQuery.ValueAsync();
      var last = await lastQuery.ValueAsync();
      var peak = (await peakQuery.ToListAsync()).FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
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
          peak.ActivePower_W / 1000
        )
        : new OzdsIotDeviceBillingData(
            source,
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
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault()
        .FutureValue();

      var lastQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault()
        .FutureValue();

      var peakQuery =
       context.QuarterHourAveragePower
        .FromSqlRaw(
            string.Format(
              PeakPowerQueryTemplate,
              string.Format(
                SchneiderBaseSubquery,
                "= {2}"
              )
            ),
            fromDate,
            toDate,
            source
        )
        .Future();

      var first = firstQuery.Value;
      var last = lastQuery.Value;
      var peak = peakQuery.ToList().FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
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
          peak.ActivePower_W / 1000
        )
        : new OzdsIotDeviceBillingData(
            source,
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
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault()
        .FutureValue();

      var lastQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault()
        .FutureValue();

      var peakQuery =
       context.QuarterHourAveragePower
        .FromSqlRaw(
            string.Format(
              PeakPowerQueryTemplate,
              string.Format(
                SchneiderBaseSubquery,
                "= {2}"
              )
            ),
            fromDate,
            toDate,
            source
        )
        .Future();

      var first = await firstQuery.ValueAsync();
      var last = await lastQuery.ValueAsync();
      var peak = (await peakQuery.ToListAsync()).FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
          source,
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
          peak.ActivePower_W / 1000
        )
        : new OzdsIotDeviceBillingData(
            source,
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
}
