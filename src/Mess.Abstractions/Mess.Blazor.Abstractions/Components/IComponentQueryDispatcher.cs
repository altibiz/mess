using Microsoft.AspNetCore.Components;

namespace Mess.Blazor.Abstractions.Components;

public interface IComponentQueryDispatcher
{
  void DispatchAsync<T>(
    AbstractComponent component,
    Func<IServiceProvider, Task<T>> fetchAsync,
    Action<T> reduce
  ) where T : class;

  void Dispatch<T>(
    AbstractComponent component,
    Func<IServiceProvider, T> fetch,
    Action<T> reduce
  ) where T : class => DispatchAsync(
    component,
    (services) => Task.FromResult(fetch(services)),
    reduce
  );
}
