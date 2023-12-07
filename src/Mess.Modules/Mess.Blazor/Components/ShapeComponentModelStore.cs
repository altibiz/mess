using System.Collections.Concurrent;
using Mess.Blazor.Abstractions.Components;
using Mess.Prelude.Extensions.Dictionaries;

// TODO: keepalive for storage so its not a memory leak in case of failure
// TODO: optimize

namespace Mess.Blazor.Components;

public class ShapeComponentModelStore : IShapeComponentModelStore
{
  private readonly
    ConcurrentDictionary<string, ConcurrentDictionary<Guid, object?>>
    _circuitStorage = new();

  private readonly ConcurrentDictionary<Guid, object?> _renderStorage = new();

  public void Add(Guid renderId, object? model)
  {
    if (model is null) return;

    _renderStorage.AddOrUpdate(
      renderId,
      _ => model,
      (_, _) => model
    );
  }

  public object? Get(Guid renderId, string? circuitId)
  {
    if (circuitId is null) return _renderStorage.GetOrDefault(renderId);

    var model = _circuitStorage
      .GetOrDefault(circuitId)
      ?.GetOrDefault(renderId);
    if (model is null && circuitId is not null)
    {
      _renderStorage.Remove(renderId, out model);
      if (model is null) return null;

      _circuitStorage.AddOrUpdate(
        circuitId,
        circuitId =>
        {
          var storage = new ConcurrentDictionary<Guid, object?>();
          storage.AddOrUpdate(
            renderId,
            _ => model,
            (_, _) => model
          );
          return storage;
        },
        (circuitId, storage) =>
        {
          storage.AddOrUpdate(
            renderId,
            _ => model,
            (_, _) => model
          );
          return storage;
        }
      );
    }

    return model;
  }

  public object? Remove(string circuitId)
  {
    _circuitStorage.Remove(circuitId, out var model);
    return model;
  }
}
