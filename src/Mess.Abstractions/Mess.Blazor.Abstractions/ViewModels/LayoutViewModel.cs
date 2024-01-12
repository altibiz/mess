using Microsoft.AspNetCore.Components;

namespace Mess.Blazor.Abstractions.ViewModels;

public class LayoutViewModel : AppViewModel
{
  public Type? ComponentType { get; set; } = default!;

  public MarkupString? Meta { get; set; }

  public MarkupString? HeadLink { get; set; }

  public MarkupString? HeadScript { get; set; }

  public MarkupString? Stylesheet { get; set; }

  public MarkupString? FootScript { get; set; }
}
