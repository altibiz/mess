namespace Mess.EventStore.Client;

public interface IEventStoreClient
{
  public Task<bool> CheckConnectionAsync();

  public bool CheckConnection();

  public void RecordEvents<T>(params object[] events) where T : class;

  public Task RecordEventsAsync<T>(params object[] events) where T : class;
}
