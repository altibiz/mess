using Marten;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Event.Abstractions.Session;
using OrchardCore.Environment.Shell;

namespace Mess.Event.Session;

public sealed class EventStoreSession
  : IEventStoreSession,
    IDisposable,
    IAsyncDisposable
{
  private IDocumentSession? _value;

  public EventStoreSession(IDocumentStore store, ShellSettings shellSettings)
  {
    _value = store.IdentitySession(shellSettings.GetDatabaseTablePrefix());
  }

  public async ValueTask DisposeAsync()
  {
    if (_value is not null)
    {
      await _value.DisposeAsync();
      _value = null;
    }
  }

  public void Dispose()
  {
    if (_value is not null)
    {
      _value.Dispose();
      _value = null;
    }
  }

  public IDocumentSession Value => _value
                                   ?? throw new InvalidOperationException(
                                     "Session has already been disposed"
                                   );
}
