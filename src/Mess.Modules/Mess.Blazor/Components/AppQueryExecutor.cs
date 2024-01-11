using System.Collections.Concurrent;
using Mess.Blazor.Abstractions.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Blazor.Components;

public class AppQueryExecutor : IAppQueryExecutor, IComponentQueryDispatcher
{
  private readonly IServiceProvider _serviceProvider;

  private readonly ILogger _logger;

  private record DispatchStorage(
    AbstractComponent Component,
    Type ResponseType,
    Func<IServiceProvider, Task<object>> FetchAsync,
    Action<object> Reduce
  );

  private readonly ConcurrentQueue<DispatchStorage> _queue = new();

  public AppQueryExecutor(
    IServiceProvider serviceProvider,
    ILogger<AppQueryExecutor> logger
  )
  {
    _serviceProvider = serviceProvider;
    _logger = logger;
  }

  public void DispatchAsync<T>(
    AbstractComponent component,
    Func<IServiceProvider, Task<T>> fetchAsync,
    Action<T> reduce
  ) where T : class
  {
    _queue.Enqueue(new DispatchStorage(
      component,
      typeof(T),
      async (services) => (await fetchAsync(services))!,
      @object =>
      {
        var response = @object as T;
        if (response is null)
        {
          throw new InvalidOperationException($"Response {response} is not of type {typeof(T)}");
        }

        reduce(response);
      }
    ));
  }

  public async Task ExecuteAsync()
  {
    List<(DispatchStorage Storage, object? response)> responses = new();

    while (_queue.TryDequeue(out var item))
    {
      object? response = null;
      try
      {
        response = await item.FetchAsync(_serviceProvider);
      }
      catch (Exception e)
      {
        _logger.LogError(
          "Query of component {} failed with {}",
          item.Component,
          e
        );
      }

      responses.Add((item, response));
    }

    // TODO: commit transactions and all that here

    foreach (var (item, response) in responses)
    {
      if (response is null)
      {
        continue;
      }

      try
      {
        item.Reduce(response);
        // TODO: expose this method somehow
        // item.Component.StateHasChanged();
      }
      catch (Exception e)
      {
        _logger.LogError(
          "Query of component {} failed with {}",
          item.Component,
          e
        );
      }
    }
  }
}
