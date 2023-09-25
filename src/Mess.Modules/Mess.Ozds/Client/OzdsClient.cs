using Mess.Ozds.Abstractions.Client;
using Mess.Ozds.Context;
using Mess.Timeseries.Abstractions.Extensions.Microsoft;
using Mess.Ozds.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mess.Ozds.Client;

public class OzdsClient : IOzdsClient
{
  public void AddAbbMeasurement(AbbMeasurement model) =>
    _services.WithTimeseriesDbContext<OzdsDbContext>(context =>
    {
      context.AbbMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });

  public Task AddAbbMeasurementAsync(AbbMeasurement model) =>
    _services.WithTimeseriesDbContextAsync<OzdsDbContext>(async context =>
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
      OzdsDbContext,
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
      OzdsDbContext,
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

  public OzdsClient(IServiceProvider services, ILogger<OzdsClient> logger)
  {
    _services = services;
    _logger = logger;
  }

  private readonly IServiceProvider _services;
  private readonly ILogger _logger;
}
