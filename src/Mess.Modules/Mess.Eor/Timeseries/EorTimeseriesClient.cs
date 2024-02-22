using Mess.Cms.Extensions.OrchardCore;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Environment.Shell;
using Z.EntityFramework.Plus;

namespace Mess.Eor.Iot;

// https://www.nuget.org/packages/Z.EntityFramework.Plus.EFCore/
// were gonna need to pack multiple queries into one request

public class EorTimeseriesClient : IEorTimeseriesClient
{
  private readonly IServiceProvider _services;
  private readonly ShellSettings _shellSettings;

  public EorTimeseriesClient(
    IServiceProvider services,
    ShellSettings shellSettings
  )
  {
    _services = services;
    _shellSettings = shellSettings;
  }

  public void AddEorMeasurement(EorMeasurement model)
  {
    _services.WithTimeseriesDbContext<EorTimeseriesDbContext>(context =>
    {
      context.EorMeasurements.Add(model.ToEntity(_shellSettings.GetTenantName()));
      context.SaveChanges();
    });
  }

  public Task AddEorMeasurementAsync(EorMeasurement model)
  {
    return _services.WithTimeseriesDbContextAsync<EorTimeseriesDbContext>(
      async context =>
      {
        context.EorMeasurements.Add(model.ToEntity(_shellSettings.GetTenantName()));
        await context.SaveChangesAsync();
      });
  }

  public void AddEorStatus(EorStatus model)
  {
    _services.WithTimeseriesDbContext<EorTimeseriesDbContext>(context =>
    {
      context.EorStatuses.Add(model.ToEntity());
      context.SaveChanges();
    });
  }

  public Task AddEorStatusAsync(EorStatus model)
  {
    return _services.WithTimeseriesDbContextAsync<EorTimeseriesDbContext>(
      async context =>
      {
        context.EorStatuses.Add(model.ToEntity());
        await context.SaveChangesAsync();
      });
  }

  public IReadOnlyList<EorMeasurement> GetEorMeasurements(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      List<EorMeasurement>
    >(context =>
    {
      return context.EorMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<IReadOnlyList<EorMeasurement>> GetEorMeasurementsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      List<EorMeasurement>
    >(async context =>
    {
      return await context.EorMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public IReadOnlyList<EorStatus> GetEorStatuses(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      List<EorStatus>
    >(context =>
    {
      return context.EorStatuses
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<IReadOnlyList<EorStatus>> GetEorStatusesAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      List<EorStatus>
    >(async context =>
    {
      return await context.EorStatuses
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }

  public (
    IReadOnlyList<EorStatus> Statuses,
    IReadOnlyList<EorMeasurement> Measurements
    ) GetEorData(string source, DateTimeOffset fromDate, DateTimeOffset toDate)
  {
    return _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      (List<EorStatus> Statuses, List<EorMeasurement> Measurements)
    >(context =>
    {
      var statuses = context.EorStatuses
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .Future();

      var measurements = context.EorMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .Future();

      return (Statuses: statuses.ToList(), Measurements: measurements.ToList());
    });
  }

  public async Task<(
    IReadOnlyList<EorStatus> Statuses,
    IReadOnlyList<EorMeasurement> Measurements
    )> GetEorDataAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      (List<EorStatus> Statuses, List<EorMeasurement> Measurements)
    >(async context =>
    {
      var statuses = context.EorStatuses
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .Future();

      var measurements = context.EorMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .Future();

      return (
        Statuses: await statuses.ToListAsync(),
        Measurements: await measurements.ToListAsync()
      );
    });
  }

  public async Task<IReadOnlyList<EorSummary>> GetEorIotDeviceSummariesAsync(
    IReadOnlyCollection<string> sources
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      IReadOnlyList<EorSummary>
    >(async context =>
    {
      var measurementQueries = sources.Select(
        source =>
        (
          DeviceId: source,
          Query: context.EorMeasurements
            .Where(measurement => measurement.Source == source)
            .OrderBy(measurement => measurement.Timestamp)
            .Select(measurement => measurement.ToModel())
            .DeferredFirstOrDefault()
            .FutureValue()
        )
      );

      var statusQueries = sources.Select(
        source =>
        (
          DeviceId: source,
          Query: context.EorStatuses
            .Where(status => status.Source == source)
            .OrderBy(status => status.Timestamp)
            .Select(status => status.ToModel())
            .DeferredFirstOrDefault()
            .FutureValue()
        )
      );

      var measurements = (
        await Task.WhenAll(
          measurementQueries.Select(
            async tuple =>
              (tuple.DeviceId, Value: await tuple.Query.ValueAsync())
          )
        )
      ).ToList();

      var statuses = (
        await Task.WhenAll(
          statusQueries.Select(
            async tuple =>
              (tuple.DeviceId, Value: await tuple.Query.ValueAsync())
          )
        )
      ).ToDictionary(tuple => tuple.DeviceId, tuple => tuple.Value);

      return measurements
        .Select(measurement =>
        {
          var status = statuses[measurement.DeviceId];
          return EorSummary.From(
            _shellSettings.Name,
            measurement.DeviceId,
            measurement.Value,
            status
          );
        })
        .Where(summary => summary != null)
        .Select(summary => summary!)
        .ToList();
    });
  }

  public IReadOnlyList<EorSummary> GetEorIotDeviceSummaries(
    IReadOnlyCollection<string> sources
  )
  {
    return _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      IReadOnlyList<EorSummary>
    >(context =>
    {
      var measurementQueries = sources.Select(
        source =>
        (
          DeviceId: source,
          Query: context.EorMeasurements
            .Where(measurement => measurement.Source == source)
            .OrderBy(measurement => measurement.Timestamp)
            .Select(measurement => measurement.ToModel())
            .DeferredFirstOrDefault()
            .FutureValue()
        )
      );

      var statusesQuery = sources.Select(
        source =>
        (
          DeviceId: source,
          Query: context.EorStatuses
            .Where(status => status.Source == source)
            .OrderBy(status => status.Timestamp)
            .Select(status => status.ToModel())
            .DeferredFirstOrDefault()
            .FutureValue()
        )
      );

      var measurements = measurementQueries
        .Select(tuple => (tuple.DeviceId, tuple.Query.Value))
        .ToList();

      var statuses = statusesQuery
        .Select(tuple => (tuple.DeviceId, tuple.Query.Value))
        .ToDictionary(tuple => tuple.DeviceId, tuple => tuple.Value);

      return measurements
        .Select(measurement =>
        {
          var status = statuses[measurement.DeviceId];
          return EorSummary.From(
            _shellSettings.Name,
            measurement.DeviceId,
            measurement.Value,
            status
          );
        })
        .Where(summary => summary != null)
        .Select(summary => summary!)
        .ToList();
    });
  }

  public async Task<EorMeasurement?> GetLastEorMeasurementAsync(
    string source
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      EorMeasurement?
    >(async context =>
    {
      return await context.EorMeasurements
        .Where(status => status.Source == source)
        .OrderBy(status => status.Timestamp)
        .Select(status => status.ToModel())
        .LastOrDefaultAsync();
    });
  }

  public EorMeasurement? GetLastEorMeasurement(string source)
  {
    return _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      EorMeasurement?
    >(context =>
    {
      return context.EorMeasurements
        .Where(status => status.Source == source)
        .OrderBy(status => status.Timestamp)
        .Select(status => status.ToModel())
        .LastOrDefault();
    });
  }

  public async Task<EorStatus?> GetEorStatusAsync(string source)
  {
    return await _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      EorStatus?
    >(async context =>
    {
      return await context.EorStatuses
        .Where(status => status.Source == source)
        .OrderBy(status => status.Timestamp)
        .Select(status => status.ToModel())
        .LastOrDefaultAsync();
    });
  }

  public EorStatus? GetEorStatus(string source)
  {
    return _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      EorStatus?
    >(context =>
    {
      return context.EorStatuses
        .Where(status => status.Source == source)
        .OrderBy(status => status.Timestamp)
        .Select(status => status.ToModel())
        .LastOrDefault();
    });
  }

  public Task<EorSummary?> GetEorIotDeviceSummaryAsync(string source)
  {
    return _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      EorSummary?
    >(async context =>
    {
      var measurementQuery = context.EorMeasurements
        .Where(measurement => measurement.Source == source)
        .OrderByDescending(measurement => measurement.Timestamp)
        .Select(status => status.ToModel())
        .DeferredFirstOrDefault()
        .FutureValue();

      var statusQuery = context.EorStatuses
        .Where(status => status.Source == source)
        .OrderByDescending(status => status.Timestamp)
        .Select(status => status.ToModel())
        .DeferredFirstOrDefault()
        .FutureValue();

      var measurement = await measurementQuery.ValueAsync();
      var status = await statusQuery.ValueAsync();
      return EorSummary.From(_shellSettings.Name, source, measurement, status);
    });
  }

  public EorSummary? GetEorIotDeviceSummary(string source)
  {
    return _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      EorSummary?
    >(context =>
    {
      var measurementQuery = context.EorMeasurements
        .Where(measurement => measurement.Source == source)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .DeferredFirstOrDefault()
        .FutureValue();

      var statusQuery = context.EorStatuses
        .Where(status => status.Source == source)
        .OrderBy(status => status.Timestamp)
        .Select(status => status.ToModel())
        .DeferredFirstOrDefault()
        .FutureValue();

      var measurement = measurementQuery.Value;
      var status = statusQuery.Value;
      return EorSummary.From(
        _shellSettings.Name,
        measurement.DeviceId,
        measurement,
        status
      );
    });
  }
}
