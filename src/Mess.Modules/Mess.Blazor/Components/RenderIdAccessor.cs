using Mess.Blazor.Abstractions.Components;

namespace Mess.Blazor.Components;

public class RenderIdAccessor : IRenderIdAccessor
{
  public Guid? RenderId { get; set; }
}
