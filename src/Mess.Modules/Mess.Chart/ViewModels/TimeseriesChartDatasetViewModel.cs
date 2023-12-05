using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.Chart.ViewModels;

public class TimeseriesChartDatasetPartViewModel
{
  [ValidateNever]
  public TimeseriesChartDatasetPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;

  public string Property { get; set; } = default!;
}
