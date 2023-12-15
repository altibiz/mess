using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Mess.Blazor.Components;

public class ComponentView : IView
{
  public string Path { get; } = "";

  public Task RenderAsync(ViewContext _) => Task.CompletedTask;
}
