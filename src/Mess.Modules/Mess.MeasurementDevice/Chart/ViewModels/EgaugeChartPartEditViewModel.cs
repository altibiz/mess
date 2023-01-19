using Mess.MeasurementDevice.Chart.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.MeasurementDevice.Chart.ViewModels;

public class EgaugeChartPartEditViewModel
{
  public EgaugeChartParameters Parameters { get; set; } = default!;

  [BindNever]
  public ContentPart Part { get; set; } = default!;

  [BindNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;
}
