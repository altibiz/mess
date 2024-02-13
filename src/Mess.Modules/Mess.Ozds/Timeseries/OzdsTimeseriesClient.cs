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

public class OzdsTimeseriesClient : IOzdsTimeseriesClient
{
  private readonly IServiceProvider _services;

  public OzdsTimeseriesClient(IServiceProvider services)
  {
    _services = services;
  }

  private const string PeakPowerQueryTemplate = """
    with
      buckets as (
        select
          time_bucket('15 minutes', "{1}") as interval,
          "{3}",
          first_value("{3}") over bucket_windows as first_value,
          last_value("{3}") over bucket_windows as last_value
        from
          "{0}"
        where
            "{1}" >= {{1}}
          and
            "{1}" < {{2}}
          and
            "{2}" = {{0}}
          window bucket_windows as (
            partition by time_bucket('15 minutes', "{1}")
            order by "{1}"
            range between unbounded preceding and unbounded following
          )
      ),
      calculation as (
        select
          (last_value - first_value) * 4 as power
        from
          buckets
      )
    select
      power as "ActivePower_W"
    from
      calculation
    order by
      power desc
    limit 1
  """;

  public void AddAbbMeasurement(AbbMeasurement model)
  {
    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.AbbMeasurements
        .Upsert(model.ToEntity())
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .NoUpdate()
        .Run();
    });
  }

  public Task AddAbbMeasurementAsync(AbbMeasurement model)
  {
    return _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        await context.AbbMeasurements
          .Upsert(model.ToEntity())
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .NoUpdate()
          .RunAsync();
      });
  }

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
        .Where(measurement => measurement.Timestamp > fromDate)
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
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

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
       context.PeakPowerQuery
        .FromSqlRaw(
            string.Format(
              PeakPowerQueryTemplate,
              nameof(OzdsTimeseriesDbContext.AbbMeasurements),
              nameof(AbbMeasurementEntity.Timestamp),
              nameof(AbbMeasurementEntity.Source),
              nameof(AbbMeasurementEntity.ActiveEnergyImportTotal_Wh)
            ),
            source,
            fromDate,
            toDate
        )
        .Future();

      var first = firstQuery.Value;
      var last = lastQuery.Value;
      var peak = peakQuery.ToList().FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
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
       context.PeakPowerQuery
        .FromSqlRaw(
            string.Format(
              PeakPowerQueryTemplate,
              nameof(OzdsTimeseriesDbContext.AbbMeasurements),
              nameof(AbbMeasurementEntity.Timestamp),
              nameof(AbbMeasurementEntity.Source),
              nameof(AbbMeasurementEntity.ActiveEnergyImportTotal_Wh)
            ),
            source,
            fromDate,
            toDate
        )
        .Future();

      var first = await firstQuery.ValueAsync();
      var last = await lastQuery.ValueAsync();
      var peak = (await peakQuery.ToListAsync()).FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
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

  public void AddSchneiderMeasurement(SchneiderMeasurement model)
  {
    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.SchneiderMeasurements
        .Upsert(model.ToEntity())
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .NoUpdate()
        .Run();
    });
  }

  public Task AddSchneiderMeasurementAsync(SchneiderMeasurement model)
  {
    return _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        await context.SchneiderMeasurements
          .Upsert(model.ToEntity())
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .NoUpdate()
          .RunAsync();
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
        .Where(measurement => measurement.Timestamp > fromDate)
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
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
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
       context.PeakPowerQuery
        .FromSqlRaw(
            string.Format(
              PeakPowerQueryTemplate,
              nameof(OzdsTimeseriesDbContext.SchneiderMeasurements),
              nameof(SchneiderMeasurementEntity.Timestamp),
              nameof(SchneiderMeasurementEntity.Source),
              nameof(SchneiderMeasurementEntity.ActiveEnergyImportTotal_Wh)
            ),
            source,
            fromDate,
            toDate
        )
        .Future();

      var first = firstQuery.Value;
      var last = lastQuery.Value;
      var peak = peakQuery.ToList().FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
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
       context.PeakPowerQuery
        .FromSqlRaw(
            string.Format(
              PeakPowerQueryTemplate,
              nameof(OzdsTimeseriesDbContext.SchneiderMeasurements),
              nameof(SchneiderMeasurementEntity.Timestamp),
              nameof(SchneiderMeasurementEntity.Source),
              nameof(SchneiderMeasurementEntity.ActiveEnergyImportTotal_Wh)
            ),
            source,
            fromDate,
            toDate
        )
        .Future();

      var first = await firstQuery.ValueAsync();
      var last = await lastQuery.ValueAsync();
      var peak = (await peakQuery.ToListAsync()).FirstOrDefault();

      return first is { } && last is { } && peak is { }
        ? new OzdsIotDeviceBillingData(
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
