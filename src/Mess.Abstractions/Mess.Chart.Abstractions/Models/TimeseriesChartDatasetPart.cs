using OrchardCore.ContentManagement;

namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartDatasetPart : ContentPart
{
  public string Property { get; set; } = default!;

  public TimeSpan History { get; set; } = default!;
}
