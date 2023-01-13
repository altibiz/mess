using Mess.EventStore.Abstractions.Events;

namespace Mess.EventStore.Abstractions.Client;

public interface IEventStoreClient
{
  public Task<bool> CheckConnectionAsync();

  public bool CheckConnection();

  public void RecordEvents<T>(params object[] events) where T : class;

  public Task RecordEventsAsync<T>(params object[] events) where T : class;

  public T? LastEvent<T>() where T : Event;

  public Task<T?> LastEventAsync<T>() where T : Event;
}
