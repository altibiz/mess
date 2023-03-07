using OrchardCore.ContentManagement.Metadata.Models;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Chart.ViewModels;

public class TimeseriesChartDatasetPartEditViewModel
{
  public string Property { get; set; } = default!;

  [ValidateNever]
  public List<SelectListItem> PropertyOptions { get; set; } = default!;

  [ValidateNever]
  public TimeseriesChartDatasetPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;
}
