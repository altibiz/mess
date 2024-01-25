using DocumentFormat.OpenXml.InkML;
using FlexLabs.EntityFrameworkCore.Upsert;
using Mess.Ozds.Abstractions.Billing;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Mess.Ozds.Timeseries;

public class OzdsTimeseriesClient : IOzdsTimeseriesClient
{
  private readonly IServiceProvider _services;

  public OzdsTimeseriesClient(IServiceProvider services)
  {
    _services = services;
  }

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
    string source
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
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel());
      var last = await query
        .FirstOrDefaultAsync();

      if (last != null)
      {
        first = await context.AbbMeasurements
          .Where(measurement => measurement.Source == source)
          .Where(measurement => measurement.Timestamp.Month == last.Timestamp.Month)
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
      string source
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
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel());
      var last = await query
        .FirstOrDefaultAsync();

      if (last != null)
      {
        first = await context.SchneiderMeasurements
          .Where(measurement => measurement.Source == source)
          .Where(measurement => measurement.Timestamp.Month == last.Timestamp.Month)
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
        .DeferredFirstOrDefault();

      var lastQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault();

      var peakQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(measurement => measurement.Milliseconds / (1000 * 60 * 15))
        .Select(
          group =>
            new
            {
              ActivePowerTotal_W = group.Average(
                measurement => measurement.ActivePowerL1_W
                  + measurement.ActivePowerL2_W
                  + measurement.ActivePowerL3_W
              )
            }
        )
        .OrderByDescending(measurement => measurement.ActivePowerTotal_W)
        .DeferredFirstOrDefault();

      var first = firstQuery.FutureValue().Value;
      var last = lastQuery.FutureValue().Value;
      var peak = peakQuery.FutureValue().Value;

      return first is null || last is null || peak is null
        ? new OzdsIotDeviceBillingData(0, 0, 0)
        : new OzdsIotDeviceBillingData(
          first!.ActiveEnergyImportTotal_Wh / 1000,
          last!.ActiveEnergyExportTotal_Wh / 1000,
          peak!.ActivePowerTotal_W / 1000
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
        .DeferredFirstOrDefault();

      var lastQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault();

      var peakQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(measurement => measurement.Milliseconds / (1000 * 60 * 15))
        .Select(
          group =>
            new
            {
              ActivePowerTotal_W = group.Average(
                measurement => measurement.ActivePowerL1_W
                  + measurement.ActivePowerL2_W
                  + measurement.ActivePowerL3_W
              )
            }
        )
        .OrderByDescending(measurement => measurement.ActivePowerTotal_W)
        .DeferredFirstOrDefault();

      var first = await firstQuery.FutureValue().ValueAsync();
      var last = await lastQuery.FutureValue().ValueAsync();
      var peak = await peakQuery.FutureValue().ValueAsync();

      return first is null || last is null || peak is null
        ? new OzdsIotDeviceBillingData(0, 0, 0)
        : new OzdsIotDeviceBillingData(
          first!.ActiveEnergyImportTotal_Wh / 1000,
          last!.ActiveEnergyExportTotal_Wh / 1000,
          peak!.ActivePowerTotal_W / 1000
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
        .DeferredFirstOrDefault();

      var lastQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault();

      var peakQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(measurement => measurement.Milliseconds / (1000 * 60 * 15))
        .Select(
          group =>
            new
            {
              ActivePowerTotal_W = group.Average(
                measurement => measurement.ActivePowerL1_W
                  + measurement.ActivePowerL2_W
                  + measurement.ActivePowerL3_W
              )
            }
        )
        .OrderByDescending(measurement => measurement.ActivePowerTotal_W)
        .DeferredFirstOrDefault();

      var first = firstQuery.FutureValue().Value;
      var last = lastQuery.FutureValue().Value;
      var peak = peakQuery.FutureValue().Value;

      return first is null || last is null || peak is null
        ? new OzdsIotDeviceBillingData(0, 0, 0)
        : new OzdsIotDeviceBillingData(
          first!.ActiveEnergyImportTotal_Wh / 1000,
          last!.ActiveEnergyExportTotal_Wh / 1000,
          peak!.ActivePowerTotal_W / 1000
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
        .DeferredFirstOrDefault();

      var lastQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault();

      var peakQuery = context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .GroupBy(measurement => measurement.Milliseconds / (1000 * 60))
        .Select(
          group =>
            new
            {
              ActivePowerTotal_kW = group.Average(
                measurement => measurement.ActivePowerL1_W
                  + measurement.ActivePowerL2_W
                  + measurement.ActivePowerL3_W
              )
            }
        )
        .OrderByDescending(measurement => measurement.ActivePowerTotal_kW)
        .DeferredFirstOrDefault();

      var first = await firstQuery.FutureValue().ValueAsync();
      var last = await lastQuery.FutureValue().ValueAsync();
      var peak = await peakQuery.FutureValue().ValueAsync();

      return first is null || last is null || peak is null
        ? new OzdsIotDeviceBillingData(0, 0, 0)
        : new OzdsIotDeviceBillingData(
          first!.ActiveEnergyImportTotal_Wh / 1000,
          last!.ActiveEnergyExportTotal_Wh / 1000,
          peak!.ActivePowerTotal_kW / 1000
        );
    });
  }
}
