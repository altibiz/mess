using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement.Metadata.Models;
using Mess.Chart.Abstractions.Models;

namespace Mess.Chart.ViewModels;

public class LineChartDatasetPartViewModel
{
  [BindNever]
  public LineChartDatasetPart Part { get; set; } = default!;

  [BindNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;
}
