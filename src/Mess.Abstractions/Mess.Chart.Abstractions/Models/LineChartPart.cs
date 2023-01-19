using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class LineChartPart : ContentPart
{
  public List<ContentItem> Datasets { get; set; } = default!;
}
