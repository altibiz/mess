using Mess.MeasurementDevice.Abstractions.Client;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.Timeseries.Abstractions.Extensions.Microsoft;
using Mess.Timeseries.Entities;
using Microsoft.Extensions.Logging;

namespace Mess.MeasurementDevice.Client;

public class MeasurementClient : IMeasurementClient
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

  public void AddEgaugeMeasurement(EgaugeMeasurementModel model) =>
    Services.WithTimeseriesContext<MeasurementDbContext>(context =>
    {
      context.EgaugeMeasurements.Add(
        new EgaugeMeasurementEntity
        {
          Tenant = model.Tenant,
          Timestamp = model.Timestamp,
          Source = model.Source,
          Voltage = model.Voltage,
          Power = model.Power
        }
      );
      context.SaveChanges();
    });

  public Task AddEgaugeMeasurementAsync(EgaugeMeasurementModel model) =>
    Services.WithTimeseriesContextAsync<MeasurementDbContext>(async context =>
    {
      context.EgaugeMeasurements.Add(
        new EgaugeMeasurementEntity
        {
          Tenant = model.Tenant,
          Timestamp = model.Timestamp,
          Source = model.Source,
          Voltage = model.Voltage,
          Power = model.Power
        }
      );
      await context.SaveChangesAsync();
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

  public MeasurementClient(
    IServiceProvider services,
    ILogger<MeasurementClient> logger
  )
  {
    Services = services;
    Logger = logger;
  }

  private IServiceProvider Services { get; }
  private ILogger Logger { get; }
}
