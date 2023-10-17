using Mess.Iot.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mess.Iot.Timeseries;

public class IotTimeseriesClient : IIotTimeseriesClient
{
  public void AddEgaugeMeasurement(EgaugeMeasurement model) =>
    _services.WithTimeseriesDbContext<IotTimeseriesDbContext>(context =>
    {
      context.EgaugeMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });

  public Task AddEgaugeMeasurementAsync(EgaugeMeasurement model) =>
    _services.WithTimeseriesDbContextAsync<IotTimeseriesDbContext>(async context =>
    {
      context.EgaugeMeasurements.Add(model.ToEntity());
      await context.SaveChangesAsync();
    });

  public IReadOnlyList<EgaugeMeasurement> GetEgaugeMeasurements(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  ) =>
    _services.WithTimeseriesDbContext<
      IotTimeseriesDbContext,
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
    DateTimeOffset beginning,
    DateTimeOffset end
  ) =>
    await _services.WithTimeseriesDbContextAsync<
      IotTimeseriesDbContext,
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

  public IotTimeseriesClient(
    IServiceProvider services,
    ILogger<IotTimeseriesClient> logger
  )
  {
    _services = services;
    _logger = logger;
  }

  private readonly IServiceProvider _services;
  private readonly ILogger _logger;
}
