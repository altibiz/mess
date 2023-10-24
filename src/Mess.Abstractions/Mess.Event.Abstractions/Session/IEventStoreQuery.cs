using Marten;

namespace Mess.Event.Abstractions.Session;

public interface IEventStoreQuery
{
  public IQuerySession Value { get; }
}
