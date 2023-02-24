using OrchardCore.ContentManagement.Metadata.Models;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Mess.Chart.ViewModels;

public class ChartPartEditViewModel
{
  public string DataProviderId { get; set; } = default!;

  [ValidateNever]
  public List<SelectListItem> ProviderIdOptions { get; set; } = default!;

  [ValidateNever]
  public List<ContentTypeDefinition> ChartContentTypes { get; set; } = default!;

  [ValidateNever]
  public ChartPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;
}
