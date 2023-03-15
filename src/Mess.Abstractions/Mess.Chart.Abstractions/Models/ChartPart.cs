using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class ChartPart : ContentPart
{
  public string DataProviderId { get; set; } = default!;

  public string ChartContentItemId { get; set; } = default!;
}
