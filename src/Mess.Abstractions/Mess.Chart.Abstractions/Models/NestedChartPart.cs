using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class NestedChartPart : ContentPart
{
  public string RootContentItemId { get; set; } = default!;
}
