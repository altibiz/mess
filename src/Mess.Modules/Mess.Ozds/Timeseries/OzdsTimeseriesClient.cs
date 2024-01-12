using DocumentFormat.OpenXml.InkML;
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
      context.AbbMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });
  }

  public Task AddAbbMeasurementAsync(AbbMeasurement model)
  {
    return _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        context.AbbMeasurements.Add(model.ToEntity());
        await context.SaveChangesAsync();
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

  public async Task<(AbbMeasurement? First, AbbMeasurement? Last)> GetAbbLastMonthMeasurementsAsync(
    string source
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      (AbbMeasurement?, AbbMeasurement?)
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
        first = await context.AbbMeasurements
          .Where(measurement => measurement.Source == source)
          .Where(measurement => measurement.Timestamp.Month == last.Timestamp.Month)
          .OrderByDescending(measurement => measurement.Timestamp)
          .Select(measurement => measurement.ToModel())
          .FirstOrDefaultAsync();

      return (first, last);
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

  public OzdsBillingData? GetAbbBillingData(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      OzdsBillingData?
    >(context =>
    {
      var firstQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(
          measurement =>
            measurement.ActiveEnergyImportTotal_kWh != null
            && measurement.ActiveEnergyImportTariff1_kWh != null
            && measurement.ActiveEnergyImportTariff2_kWh != null
        )
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault();

      var lastQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(
          measurement =>
            measurement.ActiveEnergyImportTotal_kWh != null
            && measurement.ActiveEnergyExportTariff1_kWh != null
            && measurement.ActiveEnergyImportTariff2_kWh != null
        )
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault();

      var peakQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(measurement => measurement.ActivePowerTotal_W != null)
        .GroupBy(measurement => measurement.Milliseconds / (1000 * 60 * 15))
        .Select(
          group =>
            new
            {
              ActivePowerTotal_W = group.Average(
                measurement => measurement.ActivePowerTotal_W!.Value
              )
            }
        )
        .OrderByDescending(measurement => measurement.ActivePowerTotal_W)
        .DeferredFirstOrDefault();

      var first = firstQuery.FutureValue().Value;
      var last = lastQuery.FutureValue().Value;
      var peak = peakQuery.FutureValue().Value;

      return first is null || last is null || peak is null
        ? null
        : new OzdsBillingData(
          (decimal)first.ActiveEnergyImportTotal_kWh!,
          (decimal)last.ActiveEnergyExportTotal_kWh!,
          (decimal)
          first.ActiveEnergyImportTariff1_kWh!,
          (decimal)last.ActiveEnergyImportTariff1_kWh!,
          (decimal)first.ActiveEnergyImportTariff2_kWh!,
          (decimal)last.ActiveEnergyImportTariff2_kWh!,
          (decimal)peak.ActivePowerTotal_W! / 1000
        );
    });
  }

  public async Task<OzdsBillingData?> GetAbbBillingDataAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      OzdsBillingData?
    >(async context =>
    {
      var firstQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(
          measurement =>
            measurement.ActiveEnergyImportTotal_kWh != null
            && measurement.ActiveEnergyImportTariff1_kWh != null
            && measurement.ActiveEnergyImportTariff2_kWh != null
        )
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault();

      var lastQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(
          measurement =>
            measurement.ActiveEnergyImportTotal_kWh != null
            && measurement.ActiveEnergyExportTariff1_kWh != null
            && measurement.ActiveEnergyImportTariff2_kWh != null
        )
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault();

      var peakQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(measurement => measurement.ActivePowerTotal_W != null)
        .GroupBy(measurement => measurement.Milliseconds / (1000 * 60 * 15))
        .Select(
          group =>
            new
            {
              ActivePowerTotal_W = group.Average(
                measurement => measurement.ActivePowerTotal_W!.Value
              )
            }
        )
        .OrderByDescending(measurement => measurement.ActivePowerTotal_W)
        .DeferredFirstOrDefault();

      var first = await firstQuery.FutureValue().ValueAsync();
      var last = await lastQuery.FutureValue().ValueAsync();
      var peak = await peakQuery.FutureValue().ValueAsync();

      return first is null || last is null || peak is null
        ? null
        : new OzdsBillingData(
          (decimal)first.ActiveEnergyImportTotal_kWh!,
          (decimal)last.ActiveEnergyExportTotal_kWh!,
          (decimal)
          first.ActiveEnergyImportTariff1_kWh!,
          (decimal)last.ActiveEnergyImportTariff1_kWh!,
          (decimal)first.ActiveEnergyImportTariff2_kWh!,
          (decimal)last.ActiveEnergyImportTariff2_kWh!,
          (decimal)peak.ActivePowerTotal_W! / 1000
        );
    });
  }

  public void AddSchneiderMeasurement(SchneiderMeasurement model)
  {
    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.SchneiderMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });
  }

  public Task AddSchneiderMeasurementAsync(SchneiderMeasurement model)
  {
    return _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        context.SchneiderMeasurements.Add(model.ToEntity());
        await context.SaveChangesAsync();
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

  public OzdsBillingData? GetSchneiderBillingData(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      OzdsBillingData?
    >(context =>
    {
      var firstQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(
          measurement =>
            measurement.ActiveEnergyImportTotal_kWh != null
            && measurement.ActiveEnergyImportTariff1_kWh != null
            && measurement.ActiveEnergyImportTariff2_kWh != null
        )
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault();

      var lastQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(
          measurement =>
            measurement.ActiveEnergyImportTotal_kWh != null
            && measurement.ActiveEnergyExportTariff1_kWh != null
            && measurement.ActiveEnergyImportTariff2_kWh != null
        )
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault();

      var peakQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(measurement => measurement.ActivePowerTotal_W != null)
        .GroupBy(measurement => measurement.Milliseconds / (1000 * 60 * 15))
        .Select(
          group =>
            new
            {
              ActivePowerTotal_W = group.Average(
                measurement => measurement.ActivePowerTotal_W!.Value
              )
            }
        )
        .OrderByDescending(measurement => measurement.ActivePowerTotal_W)
        .DeferredFirstOrDefault();

      var first = firstQuery.FutureValue().Value;
      var last = lastQuery.FutureValue().Value;
      var peak = peakQuery.FutureValue().Value;

      return first is null || last is null || peak is null
        ? null
        : new OzdsBillingData(
          (decimal)first.ActiveEnergyImportTotal_kWh!,
          (decimal)last.ActiveEnergyExportTotal_kWh!,
          (decimal)
          first.ActiveEnergyImportTariff1_kWh!,
          (decimal)last.ActiveEnergyImportTariff1_kWh!,
          (decimal)first.ActiveEnergyImportTariff2_kWh!,
          (decimal)last.ActiveEnergyImportTariff2_kWh!,
          (decimal)peak.ActivePowerTotal_W! / 1000
        );
    });
  }

  public async Task<OzdsBillingData?> GetSchneiderBillingDataAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      OzdsBillingData?
    >(async context =>
    {
      var firstQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(
          measurement =>
            measurement.ActiveEnergyImportTotal_kWh != null
            && measurement.ActiveEnergyImportTariff1_kWh != null
            && measurement.ActiveEnergyImportTariff2_kWh != null
        )
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredFirstOrDefault();

      var lastQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(
          measurement =>
            measurement.ActiveEnergyImportTotal_kWh != null
            && measurement.ActiveEnergyExportTariff1_kWh != null
            && measurement.ActiveEnergyImportTariff2_kWh != null
        )
        .OrderBy(measurement => measurement.Timestamp)
        .DeferredLastOrDefault();

      var peakQuery = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .Where(measurement => measurement.ActivePowerTotal_W != null)
        .GroupBy(measurement => measurement.Milliseconds / (1000 * 60 * 15))
        .Select(
          group =>
            new
            {
              ActivePowerTotal_W = group.Average(
                measurement => measurement.ActivePowerTotal_W!.Value
              )
            }
        )
        .OrderByDescending(measurement => measurement.ActivePowerTotal_W)
        .DeferredFirstOrDefault();

      var first = await firstQuery.FutureValue().ValueAsync();
      var last = await lastQuery.FutureValue().ValueAsync();
      var peak = await peakQuery.FutureValue().ValueAsync();

      return first is null || last is null || peak is null
        ? null
        : new OzdsBillingData(
          (decimal)first.ActiveEnergyImportTotal_kWh!,
          (decimal)last.ActiveEnergyExportTotal_kWh!,
          (decimal)
          first.ActiveEnergyImportTariff1_kWh!,
          (decimal)last.ActiveEnergyImportTariff1_kWh!,
          (decimal)first.ActiveEnergyImportTariff2_kWh!,
          (decimal)last.ActiveEnergyImportTariff2_kWh!,
          (decimal)peak.ActivePowerTotal_W! / 1000
        );
    });
  }
}
