using System.Collections.Concurrent;
using Mess.Blazor.Abstractions.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Blazor.Components;

public class ComponentModelStore : IShapeComponentModelStore
{
  private readonly ConcurrentDictionary<Guid, object?> _storage = new();

  public void Add(Guid id, object? model)
  {
    if (model is null) {
      return;
    }

    _storage.AddOrUpdate(
      id,
      _ => model,
      (_, _) => model
    );
  }

  public object? Get(Guid id)
  {
    return _storage.GetValueOrDefault(id);
  }

  public object? Remove(Guid id)
  {
    _storage.Remove(id, out var model);
    return model;
  }
}
