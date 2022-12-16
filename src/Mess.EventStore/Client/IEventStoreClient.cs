namespace Mess.EventStore.Client;

public interface IEventStoreClient
{
  public Task<bool> CheckConnectionAsync();

  public bool CheckConnection();
}
