using Marten;

namespace Mess.EventStore.Abstractions.Session;

public interface IEventStoreSession
{
  public IDocumentSession Value { get; }
}
