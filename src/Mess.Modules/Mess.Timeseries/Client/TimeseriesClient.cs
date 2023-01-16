using Microsoft.Extensions.Logging;
using Mess.Timeseries.Abstractions.Client;
using Mess.Timeseries.Abstractions.Extensions.Microsoft;

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
