using Marten;

namespace Mess.EventStore.Abstractions.Client;

public interface IEventStoreSession
{
  public IDocumentSession Value { get; }
}
