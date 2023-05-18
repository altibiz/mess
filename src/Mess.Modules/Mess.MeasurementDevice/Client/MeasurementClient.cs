using Mess.MeasurementDevice.Abstractions.Client;
using Mess.MeasurementDevice.Abstractions.Dispatchers;
using Mess.Timeseries.Abstractions.Extensions.Microsoft;
using Mess.Timeseries.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mess.MeasurementDevice.Client;

public class MeasurementClient : IMeasurementClient
{
  public Task<bool> CheckConnectionAsync() =>
    _services.WithTimeseriesCommandAsync(
      @"SELECT * FROM pg_stat_activity;",
      async command =>
      {
        var result = await command.ExecuteNonQueryAsync();
        var connected = true;

        LogConenction(connected);
        return connected;
      }
    );

  public bool CheckConnection() =>
    _services.WithTimeseriesCommand(
      @"SELECT * FROM pg_stat_activity;",
      command =>
      {
        var result = command.ExecuteNonQuery();
        var connected = true;

        LogConenction(connected);
        return connected;
      }
    );

  public void AddEgaugeMeasurement(DispatchedEgaugeMeasurement model) =>
    _services.WithTimeseriesContext<MeasurementDbContext>(context =>
    {
      context.EgaugeMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });

  public Task AddEgaugeMeasurementAsync(DispatchedEgaugeMeasurement model) =>
    _services.WithTimeseriesContextAsync<MeasurementDbContext>(async context =>
    {
      context.EgaugeMeasurements.Add(model.ToEntity());
      await context.SaveChangesAsync();
    });

  public IReadOnlyList<DispatchedEgaugeMeasurement> GetEgaugeMeasurements(
    string source,
    DateTime beginning,
    DateTime end
  ) =>
    _services.WithTimeseriesContext<
      MeasurementDbContext,
      List<DispatchedEgaugeMeasurement>
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
    IReadOnlyList<DispatchedEgaugeMeasurement>
  > GetEgaugeMeasurementsAsync(
    string source,
    DateTime beginning,
    DateTime end
  ) =>
    await _services.WithTimeseriesContextAsync<
      MeasurementDbContext,
      List<DispatchedEgaugeMeasurement>
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

  private void LogConenction(bool connected)
  {
    if (connected)
    {
      _logger.LogInformation("Connected to Timeseries server");
    }
    else
    {
      _logger.LogInformation("Failed connecting to Timeseries server");
    }
  }

  public MeasurementClient(
    IServiceProvider services,
    ILogger<MeasurementClient> logger
  )
  {
    _services = services;
    _logger = logger;
  }

  private readonly IServiceProvider _services;
  private readonly ILogger _logger;
}
