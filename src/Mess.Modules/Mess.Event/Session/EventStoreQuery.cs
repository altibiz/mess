using Marten;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Event.Abstractions.Session;
using OrchardCore.Environment.Shell;

namespace Mess.Event.Session;

public sealed class EventStoreQuery
  : IEventStoreQuery,
    IDisposable,
    IAsyncDisposable
{
  private IQuerySession? _value;

  public EventStoreQuery(IDocumentStore store, ShellSettings shellSettings)
  {
    _value = store.QuerySession(shellSettings.GetDatabaseTablePrefix());
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

  public IQuerySession Value => _value
                                ?? throw new InvalidOperationException(
                                  "Query has already been disposed");
}
