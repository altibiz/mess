using OrchardCore.ContentManagement.Metadata.Models;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Mess.Chart.ViewModels;

public class LineChartPartThumbnailViewModel
{
  [ValidateNever]
  public LineChartPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;
}
