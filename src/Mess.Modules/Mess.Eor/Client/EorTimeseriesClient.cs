using Mess.Eor.Abstractions.Client;
using Mess.Eor.Context;
using Mess.Eor.Entities;
using Mess.Timeseries.Abstractions.Extensions.Microsoft;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;
using Z.EntityFramework.Plus;

namespace Mess.MeasurementDevice.Client;

// https://www.nuget.org/packages/Z.EntityFramework.Plus.EFCore/
// were gonna need to pack multiple queries into one request

public class EorTimeseriesClient : IEorTimeseriesClient
{
  public void AddEorMeasurement(EorMeasurement model) =>
    _services.WithTimeseriesDbContext<EorTimeseriesDbContext>(context =>
    {
      context.EorMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });

  public Task AddEorMeasurementAsync(EorMeasurement model) =>
    _services.WithTimeseriesDbContextAsync<EorTimeseriesDbContext>(async context =>
    {
      context.EorMeasurements.Add(model.ToEntity());
      await context.SaveChangesAsync();
    });

  public void AddEorStatus(EorStatus model) =>
    _services.WithTimeseriesDbContext<EorTimeseriesDbContext>(context =>
    {
      context.EorStatuses.Add(model.ToEntity());
      context.SaveChanges();
    });

  public Task AddEorStatusAsync(EorStatus model) =>
    _services.WithTimeseriesDbContextAsync<EorTimeseriesDbContext>(async context =>
    {
      context.EorStatuses.Add(model.ToEntity());
      await context.SaveChangesAsync();
    });

  public IReadOnlyList<EorMeasurement> GetEorMeasurements(
    string source,
    DateTime beginning,
    DateTime end
  ) =>
    _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      List<EorMeasurement>
    >(context =>
    {
      return context.EorMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });

  public async Task<IReadOnlyList<EorMeasurement>> GetEorMeasurementsAsync(
    string source,
    DateTime beginning,
    DateTime end
  ) =>
    await _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      List<EorMeasurement>
    >(async context =>
    {
      return await context.EorMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });

  public IReadOnlyList<EorStatus> GetEorStatuses(
    string source,
    DateTime beginning,
    DateTime end
  ) =>
    _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      List<EorStatus>
    >(context =>
    {
      return context.EorStatuses
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });

  public async Task<IReadOnlyList<EorStatus>> GetEorStatusesAsync(
    string source,
    DateTime beginning,
    DateTime end
  ) =>
    await _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      List<EorStatus>
    >(async context =>
    {
      return await context.EorStatuses
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });

  public (
    IReadOnlyList<EorStatus> Statuses,
    IReadOnlyList<EorMeasurement> Measurements
  ) GetEorData(string source, DateTime beginning, DateTime end) =>
    _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      (List<EorStatus> Statuses, List<EorMeasurement> Measurements)
    >(context =>
    {
      var statuses = context.EorStatuses
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .Future();

      var measurements = context.EorMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .Future();

      return (Statuses: statuses.ToList(), Measurements: measurements.ToList());
    });

  public async Task<(
    IReadOnlyList<EorStatus> Statuses,
    IReadOnlyList<EorMeasurement> Measurements
  )> GetEorDataAsync(string source, DateTime beginning, DateTime end) =>
    await _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      (List<EorStatus> Statuses, List<EorMeasurement> Measurements)
    >(async context =>
    {
      var statuses = context.EorStatuses
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .Future();

      var measurements = context.EorMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .Future();

      return (
        Statuses: await statuses.ToListAsync(),
        Measurements: await measurements.ToListAsync()
      );
    });

  public async Task<
    IReadOnlyList<EorMeasurementDeviceSummary>
  > GetEorMeasurementDeviceSummariesAsync(
    IReadOnlyCollection<string> sources
  ) =>
    await _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      IReadOnlyList<EorMeasurementDeviceSummary>
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
            async (tuple) =>
              (DeviceId: tuple.DeviceId, Value: await tuple.Query.ValueAsync())
          )
        )
      ).ToList();

      var statuses = (
        await Task.WhenAll(
          statusQueries.Select(
            async tuple =>
              (DeviceId: tuple.DeviceId, Value: await tuple.Query.ValueAsync())
          )
        )
      ).ToDictionary(tuple => tuple.DeviceId, tuple => tuple.Value);

      return measurements
        .Select(measurement =>
        {
          var status = statuses[measurement.DeviceId];
          return EorMeasurementDeviceSummary.From(
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

  public IReadOnlyList<EorMeasurementDeviceSummary> GetEorMeasurementDeviceSummaries(
    IReadOnlyCollection<string> sources
  ) =>
    _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      IReadOnlyList<EorMeasurementDeviceSummary>
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
        .Select(tuple => (DeviceId: tuple.DeviceId, Value: tuple.Query.Value))
        .ToList();

      var statuses = statusesQuery
        .Select(tuple => (DeviceId: tuple.DeviceId, Value: tuple.Query.Value))
        .ToDictionary(tuple => tuple.DeviceId, tuple => tuple.Value);

      return measurements
        .Select(measurement =>
        {
          var status = statuses[measurement.DeviceId];
          return EorMeasurementDeviceSummary.From(
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

  public async Task<EorMeasurement?> GetLastEorMeasurementAsync(
    string source
  ) =>
    await _services.WithTimeseriesDbContextAsync<
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

  public EorMeasurement? GetLastEorMeasurement(string source) =>
    _services.WithTimeseriesDbContext<
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

  public async Task<EorStatus?> GetEorStatusAsync(string source) =>
    await _services.WithTimeseriesDbContextAsync<
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

  public EorStatus? GetEorStatus(string source) =>
    _services.WithTimeseriesDbContext<
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

  public Task<EorMeasurementDeviceSummary?> GetEorMeasurementDeviceSummaryAsync(
    string source
  ) =>
    _services.WithTimeseriesDbContextAsync<
      EorTimeseriesDbContext,
      EorMeasurementDeviceSummary?
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
      return EorMeasurementDeviceSummary.From(
        _shellSettings.Name,
        source,
        measurement,
        status
      );
    });

  public EorMeasurementDeviceSummary? GetEorMeasurementDeviceSummary(
    string source
  ) =>
    _services.WithTimeseriesDbContext<
      EorTimeseriesDbContext,
      EorMeasurementDeviceSummary?
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
      return EorMeasurementDeviceSummary.From(
        _shellSettings.Name,
        measurement.DeviceId,
        measurement,
        status
      );
    });

  public EorTimeseriesClient(
    IServiceProvider services,
    ILogger<EorTimeseriesClient> logger,
    ShellSettings shellSettings
  )
  {
    _services = services;
    _logger = logger;
    _shellSettings = shellSettings;
  }

  private readonly IServiceProvider _services;
  private readonly ILogger _logger;
  private readonly ShellSettings _shellSettings;
}
