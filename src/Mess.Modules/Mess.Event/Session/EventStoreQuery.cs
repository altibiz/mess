using Microsoft.Extensions.DependencyInjection;
using Marten;
using Mess.Event.Abstractions.Session;
using OrchardCore.Environment.Shell;
using Mess.Cms.Extensions.OrchardCore;

namespace Mess.Event.Session;

public sealed class EventStoreQuery
  : IEventStoreQuery,
    IDisposable,
    IAsyncDisposable
{
  public IQuerySession Value => _value
      ?? throw new InvalidOperationException("Query has already been disposed");

  public EventStoreQuery(IDocumentStore store, ShellSettings shellSettings)
  {
    _value = store.QuerySession(shellSettings.GetDatabaseTablePrefix());
  }

  private IQuerySession? _value;

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
