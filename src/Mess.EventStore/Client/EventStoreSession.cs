using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell.Scope;
using Mess.Util.Extensions.OrchardCore;
using Marten;

namespace Mess.EventStore.Client;

internal sealed class EventStoreSession : IDisposable, IAsyncDisposable
{
  public IDocumentSession Value
  {
    get =>
      _value
      ?? throw new InvalidOperationException(
        "Session has already been disposed"
      );
  }

  public EventStoreSession(IServiceProvider services)
  {
    var store = services.GetRequiredService<IDocumentStore>();
    _value = store.OpenSession(ShellScope.Current.GetTenantName());
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

internal static class EventStoreSessionServiceProviederExtensions
{
  public static async Task<T> WithEventStoreSessionAsync<T>(
    this IServiceProvider services,
    Func<IDocumentSession, Task<T>> todo
  )
  {
    await using var scope = services.CreateAsyncScope();
    var session = scope.ServiceProvider.GetRequiredService<EventStoreSession>();
    return await todo(session.Value);
  }

  public static T WithEventStoreSession<T>(
    this IServiceProvider services,
    Func<IDocumentSession, T> todo
  )
  {
    using var scope = services.CreateScope();
    var session = scope.ServiceProvider.GetRequiredService<EventStoreSession>();
    return todo(session.Value);
  }
}
