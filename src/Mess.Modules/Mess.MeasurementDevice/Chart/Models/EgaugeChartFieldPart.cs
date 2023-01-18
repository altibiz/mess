using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Chart.Models;

public class EgaugeChartFieldPart : ContentPart
{
  public string Label { get; set; } = default!;

  public string Unit { get; set; } = default!;

  public string Color { get; set; } = default!;

  public string Field { get; set; } = default!;
}
