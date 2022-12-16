namespace Mess.Timeseries.Client;

public interface ITimeseriesClient
{
  public Task<bool> CheckConnectionAsync();

  public bool CheckConnection();
}
