using Mess.Timeseries.Entities;
using Microsoft.Extensions.Logging;

namespace Mess.Timeseries.Client;

public class TimeseriesClient : ITimeseriesClient
{
  public Task<bool> CheckConnectionAsync() =>
    Services.WithTimeseriesCommandAsync(
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
    Services.WithTimeseriesCommand(
      @"SELECT * FROM pg_stat_activity;",
      command =>
      {
        var result = command.ExecuteNonQuery();
        var connected = true;

        LogConenction(connected);
        return connected;
      }
    );

  private void LogConenction(bool connected)
  {
    if (connected)
    {
      Logger.LogInformation("Connected to Timeseries server");
    }
    else
    {
      Logger.LogInformation("Failed connecting to Timeseries server");
    }
  }

  public Task AddMeasurementAsync(Measurement measurement) =>
    Services.WithTimeseriesContextAsync(async context =>
    {
      await context.Measurements.AddAsync(measurement);
      await context.SaveChangesAsync();

      return true;
    });

  public void AddMeasurement(Measurement measurement) =>
    Services.WithTimeseriesContext(context =>
    {
      context.Measurements.Add(measurement);
      context.SaveChanges();

      return true;
    });

  public TimeseriesClient(
    IServiceProvider services,
    ILogger<TimeseriesClient> logger
  )
  {
    Services = services;
    Logger = logger;
  }

  private IServiceProvider Services { get; }
  private ILogger Logger { get; }
}
