namespace Mess.Blazor.Abstractions.Components;

public interface IRenderIdAccessor
{
  public Guid? RenderId { get; set; }
}
