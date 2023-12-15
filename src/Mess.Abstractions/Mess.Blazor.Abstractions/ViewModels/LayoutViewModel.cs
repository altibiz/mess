namespace Mess.Blazor.Abstractions.ViewModels;

public class LayoutViewModel
{
  public Type ComponentType { get; set; } = default!;

  public Guid RenderId { get; set; } = default!;
}
