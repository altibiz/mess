using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Mess.Blazor.Components;

// TODO: take notes from RazorView

public class ComponentView : IView
{
  private readonly Type _componentType;

  private readonly object? _model;

  public ComponentView(Type componentType, object? model)
  {
    _componentType = componentType;
    _model = model;
  }

  public string Path { get; } = "";

  public Task RenderAsync(ViewContext viewContext)
  {
    return Task.CompletedTask;
  }
}
