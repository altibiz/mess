namespace Mess.Blazor.Abstractions.ViewModels;

public class ComponentViewModel
{
  public Type ComponentType { get; set; } = default!;

  public object? Model { get; set; } = default!;
}
