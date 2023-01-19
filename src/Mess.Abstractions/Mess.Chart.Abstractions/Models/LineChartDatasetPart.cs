using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class LineChartDatasetPart : ContentPart
{
  public string Label { get; set; } = default!;

  public string Color { get; set; } = default!;

  public ContentItem Dataset { get; set; } = default!;
}
