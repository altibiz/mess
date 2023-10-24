using Marten;

namespace Mess.Event.Abstractions.Session;

public interface IEventStoreSession
{
  public IDocumentSession Value { get; }
}
