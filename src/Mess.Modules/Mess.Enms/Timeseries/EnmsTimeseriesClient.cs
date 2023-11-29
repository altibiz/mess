using Mess.Enms.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mess.Enms.Timeseries;

public class EnmsTimeseriesClient : IEnmsTimeseriesClient
{
  public void AddEgaugeMeasurement(EgaugeMeasurement model) =>
    _services.WithTimeseriesDbContext<EnmsTimeseriesDbContext>(context =>
    {
      context.EgaugeMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });

  public Task AddEgaugeMeasurementAsync(EgaugeMeasurement model) =>
    _services.WithTimeseriesDbContextAsync<EnmsTimeseriesDbContext>(async context =>
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
      EnmsTimeseriesDbContext,
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
      EnmsTimeseriesDbContext,
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

  public EnmsTimeseriesClient(
    IServiceProvider services
  )
  {
    _services = services;
  }

  private readonly IServiceProvider _services;
}
