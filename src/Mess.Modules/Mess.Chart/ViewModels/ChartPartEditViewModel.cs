using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.Chart.ViewModels;

public class ChartPartEditViewModel
{
  [ValidateNever] public ChartPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;

  [ValidateNever]
  public List<SelectListItem> ChartContentItemIdOptions { get; set; } =
    default!;

  public string ChartContentItemId { get; set; } = default!;
}
