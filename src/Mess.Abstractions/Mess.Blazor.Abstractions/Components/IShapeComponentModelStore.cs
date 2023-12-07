using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Blazor.Abstractions.Components;

public interface IShapeComponentModelStore
{
  void Add(Guid renderId, object? model);

  object? Get(Guid renderId, string? circuitId);

  object? Remove(string circuitId);
}
