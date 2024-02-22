using Mess.Cms.Extensions.OrchardCore;
using Mess.Enms.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using OrchardCore.Environment.Shell;

namespace Mess.Enms.Timeseries;

public class EnmsTimeseriesClient : IEnmsTimeseriesClient
{
  private readonly IServiceProvider _services;
  private readonly ShellSettings _shellSettings;

  public EnmsTimeseriesClient(
    IServiceProvider services,
    ShellSettings shellSettings
  )
  {
    _services = services;
    _shellSettings = shellSettings;
  }

  public void AddEgaugeMeasurement(EgaugeMeasurement model)
  {
    _services.WithTimeseriesDbContext<EnmsTimeseriesDbContext>(context =>
    {
      context.EgaugeMeasurements.Add(model.ToEntity(_shellSettings.GetDatabaseTablePrefix()));
      context.SaveChanges();
    });
  }

  public Task AddEgaugeMeasurementAsync(EgaugeMeasurement model)
  {
    return _services.WithTimeseriesDbContextAsync<EnmsTimeseriesDbContext>(
      async context =>
      {
        context.EgaugeMeasurements.Add(model.ToEntity(_shellSettings.GetDatabaseTablePrefix()));
        await context.SaveChangesAsync();
      });
  }

  public IReadOnlyList<EgaugeMeasurement> GetEgaugeMeasurements(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return _services.WithTimeseriesDbContext<
      EnmsTimeseriesDbContext,
      List<EgaugeMeasurement>
    >(context =>
    {
      return context.EgaugeMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });
  }

  public async Task<
    IReadOnlyList<EgaugeMeasurement>
  > GetEgaugeMeasurementsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      EnmsTimeseriesDbContext,
      List<EgaugeMeasurement>
    >(async context =>
    {
      return await context.EgaugeMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > fromDate)
        .Where(measurement => measurement.Timestamp < toDate)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });
  }
}
