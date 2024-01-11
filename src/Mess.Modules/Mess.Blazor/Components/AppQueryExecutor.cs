using System.Collections.Concurrent;
using Mess.Blazor.Abstractions.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using YesSql;

// NOTE: figure out how to hook up into blazor rendering
// there is oninitialized async and that is ok but the execution has to wait
// for all dispatches to finish - so that should somehow run after all the
// oninitializedasync from all components finished
// + differentiate that from event handling cuz sometimes multiple components
// handle one event and can both do some db querying when that happend and we
// should also "bundle" thos as well
// these 2 things should cover 99% of cases of making transactions work properly
// so solution is to read a bunch of aspnetcore code wee

// NOTE: since we have a component base class we should more finely integrate
// all this with them - like instead of oninitializedasync have some kind of
// query building/reducing props/methods and call them in the oninitializedasync
// also for all app components make this seamless with an app component base
// class because that is also a special type of component

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

    // TODO: init transactions and all that here

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
