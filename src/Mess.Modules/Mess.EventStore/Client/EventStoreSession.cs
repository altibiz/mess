using Microsoft.Extensions.DependencyInjection;
using Marten;
using Mess.Tenants;
using Mess.EventStore.Abstractions.Client;

namespace Mess.EventStore.Client;

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

  public EventStoreSession(IServiceProvider services, ITenantProvider tenant)
  {
    var store = services.GetRequiredService<IDocumentStore>();
    _value = store.OpenSession(tenant.GetTenantName());
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
