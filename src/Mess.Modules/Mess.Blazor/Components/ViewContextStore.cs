using System.Collections.Concurrent;
using Mess.Blazor.Abstractions.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Blazor.Components;

public class ViewContextStore : IViewContextStore
{
  private readonly ConcurrentDictionary<Guid, ViewContext> _storage = new();

  public void Add(Guid id, ViewContext viewContext)
  {
    _storage.AddOrUpdate(
      id,
      _ => viewContext,
      (_, _) => viewContext
    );
  }

  public ViewContext? Get(Guid id)
  {
    return _storage.GetValueOrDefault(id);
  }

  public ViewContext? Remove(Guid id)
  {
    _storage.Remove(id, out var viewContext);
    return viewContext;
  }
}
