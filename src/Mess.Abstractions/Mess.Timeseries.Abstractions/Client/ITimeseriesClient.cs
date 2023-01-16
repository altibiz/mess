namespace Mess.Timeseries.Abstractions.Client;

public interface ITimeseriesClient
{
  public Task<bool> CheckConnectionAsync();

  public bool CheckConnection();
}
