using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Blazor.Abstractions.Components;

public interface IViewContextStore
{
  void Add(Guid id, ViewContext viewContext);

  ViewContext? Get(Guid id);

  ViewContext? Remove(Guid id);
}
