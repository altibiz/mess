using OrchardCore.ContentManagement;

namespace Mess.Chart.Models;

public class ChartPart : ContentPart
{
  public string Type { get; set; } = default!;
}
