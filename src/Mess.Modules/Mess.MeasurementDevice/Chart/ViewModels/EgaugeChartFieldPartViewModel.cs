using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Chart.ViewModels;

public class EgaugeChartFieldPartViewModel
{
  public string Label { get; set; } = default!;

  public string Unit { get; set; } = default!;

  public string Color { get; set; } = default!;

  public string Field { get; set; } = default!;

  [BindNever]
  public ContentPart Part { get; set; } = default!;
}
