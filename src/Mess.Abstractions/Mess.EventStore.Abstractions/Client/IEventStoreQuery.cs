using Marten;

namespace Mess.EventStore.Abstractions.Client;

public interface IEventStoreQuery
{
  public IQuerySession Value { get; }
}
