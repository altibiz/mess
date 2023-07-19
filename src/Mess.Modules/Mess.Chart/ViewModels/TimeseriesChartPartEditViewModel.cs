using OrchardCore.ContentManagement.Metadata.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Chart.ViewModels;

public class TimeseriesChartPartEditViewModel
{
  [ValidateNever]
  public TimeseriesChartPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;

  public string ChartDataProviderId { get; set; } = default!;

  [ValidateNever]
  public List<SelectListItem> ChartDataProviderIdOptions { get; set; } =
    default!;
}