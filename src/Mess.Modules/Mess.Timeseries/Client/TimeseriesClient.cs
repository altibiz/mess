using Mess.Timeseries.Abstractions.Client;
using Microsoft.Extensions.Logging;
using AbstractMeasurement = Mess.Timeseries.Abstractions.Entities.Measurement;

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

  public Task AddMeasurementAsync(AbstractMeasurement measurement) =>
    Services.WithTimeseriesContextAsync(async context =>
    {
      await context.Measurements.AddAsync(
        new()
        {
          Tenant = measurement.tenant,
          SourceId = measurement.sourceId,
          Timestamp = measurement.timestamp,
          Power = measurement.power,
          Voltage = measurement.voltage
        }
      );
      await context.SaveChangesAsync();

      return true;
    });

  public void AddMeasurement(AbstractMeasurement measurement) =>
    Services.WithTimeseriesContext(context =>
    {
      context.Measurements.Add(
        new()
        {
          Tenant = measurement.tenant,
          SourceId = measurement.sourceId,
          Timestamp = measurement.timestamp,
          Power = measurement.power,
          Voltage = measurement.voltage
        }
      );
      context.SaveChanges();

      return true;
    });

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
