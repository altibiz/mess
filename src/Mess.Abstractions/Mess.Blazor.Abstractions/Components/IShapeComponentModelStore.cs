using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Blazor.Abstractions.Components;

public interface IShapeComponentModelStore
{
  void Add(Guid id, object? model);

  object? Get(Guid id);

  object? Remove(Guid id);
}
