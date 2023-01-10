using Microsoft.Extensions.DependencyInjection;
using Marten;
using Mess.Util.OrchardCore.Tenants;

namespace Mess.EventStore.Client;

public sealed class EventStoreQuery : IDisposable, IAsyncDisposable
{
  public IQuerySession Value
  {
    get =>
      _value
      ?? throw new InvalidOperationException("Query has already been disposed");
  }

  public EventStoreQuery(IServiceProvider services, ITenantProvider tenant)
  {
    var store = services.GetRequiredService<IDocumentStore>();
    _value = store.QuerySession(tenant.GetTenantName());
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

internal static class EventStoreQueryServiceProviederExtensions
{
  public static async Task<T> WithEventStoreQueryAsync<T>(
    this IServiceProvider services,
    Func<IQuerySession, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var session = scope.ServiceProvider.GetRequiredService<EventStoreQuery>();
    return await todo(session.Value);
  }

  public static T WithEventStoreQuery<T>(
    this IServiceProvider services,
    Func<IQuerySession, T> todo
  )
  {
    using var scope = services.CreateScope();
    var session = scope.ServiceProvider.GetRequiredService<EventStoreQuery>();
    return todo(session.Value);
  }
}
