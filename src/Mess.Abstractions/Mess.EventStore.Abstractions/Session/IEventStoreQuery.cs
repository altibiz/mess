using Marten;

namespace Mess.EventStore.Abstractions.Session;

public interface IEventStoreQuery
{
  public IQuerySession Value { get; }
}
