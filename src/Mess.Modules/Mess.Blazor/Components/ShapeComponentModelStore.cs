using System.Collections.Concurrent;
using Mess.Blazor.Abstractions.Components;
using Mess.Prelude.Extensions.Dictionaries;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Blazor.Components;

// TODO: check if renderStorage is a memory leak

public class ShapeComponentModelStore : IShapeComponentModelStore
{
  private readonly ConcurrentDictionary<Guid, object?> _renderStorage = new();
  private readonly ConcurrentDictionary<string, object?> _circuitStorage = new();

  public void Add(Guid renderId, object? model)
  {
    if (model is null) {
      return;
    }

    _renderStorage.AddOrUpdate(
      renderId,
      _ => model,
      (_, _) => model
    );
  }

  public object? Get(Guid renderId, string? circuitId)
  {
    if (circuitId is null)
    {
      return _renderStorage.GetOrDefault(renderId);
    }

    var model = _circuitStorage.GetOrDefault(circuitId);
    if (model is null && circuitId is not null)
    {
      _renderStorage.Remove(renderId, out model);
      if (model is null)
      {
        return null;
      }

      _circuitStorage.AddOrUpdate(
        circuitId,
        _ => model,
        (_, _) => model
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
