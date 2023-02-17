namespace Mess.Chart.Abstractions.Models;

public class TimeseriesChartDatasetPart : NestedChartPart
{
  public string Property { get; set; } = default!;

  public TimeSpan History { get; set; } = default!;
}
