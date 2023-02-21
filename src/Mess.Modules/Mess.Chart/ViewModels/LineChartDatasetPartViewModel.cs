using OrchardCore.ContentManagement.Metadata.Models;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Mess.Chart.ViewModels;

public class LineChartDatasetPartViewModel
{
  [ValidateNever]
  public string Label { get; set; } = default!;

  [ValidateNever]
  public int Index { get; set; } = default!;

  [ValidateNever]
  public LineChartDatasetPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;
}
