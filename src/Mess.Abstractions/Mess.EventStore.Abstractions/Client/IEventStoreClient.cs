using Mess.EventStore.Abstractions.Events;

namespace Mess.EventStore.Abstractions.Client;

public interface IEventStoreClient
{
  public Task<bool> CheckConnectionAsync();

  public bool CheckConnection();

  public void RecordEvents<T>(params IEvent[] events) where T : class;

  public Task RecordEventsAsync<T>(params IEvent[] events) where T : class;
}
