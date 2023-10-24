using Marten;
using Mess.Event.Abstractions.Session;
using OrchardCore.Environment.Shell;
using Mess.OrchardCore.Extensions.OrchardCore;

namespace Mess.Event.Session;

public sealed class EventStoreSession
  : IEventStoreSession,
    IDisposable,
    IAsyncDisposable
{
  public IDocumentSession Value
  {
    get =>
      _value
      ?? throw new InvalidOperationException(
        "Session has already been disposed"
      );
  }

  public EventStoreSession(IDocumentStore store, ShellSettings shellSettings)
  {
    _value = store.IdentitySession(shellSettings.GetDatabaseTablePrefix());
  }

  private IDocumentSession? _value = null;

  public void Dispose()
  {
    if (_value is not null)
    {
      _value.Dispose();
      _value = null;
    }
  }

  public async ValueTask DisposeAsync()
  {
    if (_value is not null)
    {
      await _value.DisposeAsync();
      _value = null;
    }
  }
}
