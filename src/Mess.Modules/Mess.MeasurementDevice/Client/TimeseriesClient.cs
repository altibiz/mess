using Mess.MeasurementDevice.Abstractions.Client;
using Mess.MeasurementDevice.Context;
using Mess.Timeseries.Abstractions.Extensions.Microsoft;
using Mess.MeasurementDevice.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mess.MeasurementDevice.Client;

public class TimeseriesClient : ITimeseriesClient
{
  public void AddEgaugeMeasurement(EgaugeMeasurement model) =>
    _services.WithTimeseriesDbContext<MeasurementDbContext>(context =>
    {
      context.EgaugeMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });

  public Task AddEgaugeMeasurementAsync(EgaugeMeasurement model) =>
    _services.WithTimeseriesDbContextAsync<MeasurementDbContext>(async context =>
    {
      context.EgaugeMeasurements.Add(model.ToEntity());
      await context.SaveChangesAsync();
    });

  public IReadOnlyList<EgaugeMeasurement> GetEgaugeMeasurements(
    string source,
    DateTime beginning,
    DateTime end
  ) =>
    _services.WithTimeseriesDbContext<
      MeasurementDbContext,
      List<EgaugeMeasurement>
    >(context =>
    {
      return context.EgaugeMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });

  public async Task<
    IReadOnlyList<EgaugeMeasurement>
  > GetEgaugeMeasurementsAsync(
    string source,
    DateTime beginning,
    DateTime end
  ) =>
    await _services.WithTimeseriesDbContextAsync<
      MeasurementDbContext,
      List<EgaugeMeasurement>
    >(async context =>
    {
      return await context.EgaugeMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });

  public void AddAbbMeasurement(AbbMeasurement model) =>
    _services.WithTimeseriesDbContext<MeasurementDbContext>(context =>
    {
      context.AbbMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });

  public Task AddAbbMeasurementAsync(AbbMeasurement model) =>
    _services.WithTimeseriesDbContextAsync<MeasurementDbContext>(async context =>
    {
      context.AbbMeasurements.Add(model.ToEntity());
      await context.SaveChangesAsync();
    });

  public IReadOnlyList<AbbMeasurement> GetAbbMeasurements(
    string source,
    DateTime beginning,
    DateTime end
  ) =>
    _services.WithTimeseriesDbContext<
      MeasurementDbContext,
      List<AbbMeasurement>
    >(context =>
    {
      return context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });

  public async Task<IReadOnlyList<AbbMeasurement>> GetAbbMeasurementsAsync(
    string source,
    DateTime beginning,
    DateTime end
  ) =>
    await _services.WithTimeseriesDbContextAsync<
      MeasurementDbContext,
      List<AbbMeasurement>
    >(async context =>
    {
      return await context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });

  public TimeseriesClient(
    IServiceProvider services,
    ILogger<TimeseriesClient> logger
  )
  {
    _services = services;
    _logger = logger;
  }

  private readonly IServiceProvider _services;
  private readonly ILogger _logger;
}
