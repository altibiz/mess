using Microsoft.Extensions.DependencyInjection;
using Marten;
using Mess.Tenants;
using Mess.EventStore.Abstractions.Client;

namespace Mess.EventStore.Client;

public sealed class EventStoreQuery
  : IEventStoreQuery,
    IDisposable,
    IAsyncDisposable
{
  public IQuerySession Value
  {
    get =>
      _value
      ?? throw new InvalidOperationException("Query has already been disposed");
  }

  public EventStoreQuery(IServiceProvider services, ITenants tenants)
  {
    var store = services.GetRequiredService<IDocumentStore>();
    _value = store.QuerySession(tenants.Current.Name);
  }

  private IQuerySession? _value = null;

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
