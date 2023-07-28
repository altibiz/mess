using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class ChartPart : ContentPart
{
  public string ChartContentItemId { get; set; } = default!;
}
