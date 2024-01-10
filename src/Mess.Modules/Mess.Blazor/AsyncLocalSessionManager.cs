using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data;
using OrchardCore.Data.Documents;
using OrchardCore.Environment.Shell.Scope;
using YesSql;

namespace Mess.Blazor;

public class AsyncLocalSessionManager
{
  private readonly IStore _store;

  private readonly IScopedIndexProvider[] _scopedIndexProviders;

  private class SessionStorage
  {
    public ISession Session { get; set; } = default!;

    public int Id { get; } = Interlocked.Increment(ref _currentId);

    private static int _currentId = 0;
  }

  private readonly AsyncLocal<SessionStorage> _sessionStorage = new();

  public AsyncLocalSessionManager(
    IStore store,
    IEnumerable<IScopedIndexProvider> scopedIndexProviders
  )
  {
    _store = store;
    _scopedIndexProviders = scopedIndexProviders.ToArray();

    ShellScope.Current
      .RegisterBeforeDispose(scope =>
      {
        return scope.ServiceProvider
          .GetRequiredService<IDocumentStore>()
          .CommitAsync();
      })
      .AddExceptionHandler((scope, e) =>
      {
        return scope.ServiceProvider
          .GetRequiredService<IDocumentStore>()
          .CancelAsync();
      });
  }

  public ISession Session
  {
    get
    {
      if (_sessionStorage.Value is null)
      {
        var session = _store.CreateSession();

        session.RegisterIndexes(_scopedIndexProviders.ToArray());

        _sessionStorage.Value = new()
        {
          Session = session
        };
      }

      return _sessionStorage.Value.Session;
    }
  }
}
