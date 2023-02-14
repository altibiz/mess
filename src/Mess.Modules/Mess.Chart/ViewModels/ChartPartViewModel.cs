using OrchardCore.ContentManagement.Metadata.Models;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Mess.Chart.ViewModels;

public class ChartPartViewModel
{
  public string DataProviderId { get; set; } = default!;

  [ValidateNever]
  public List<SelectListItem> ProviderIds { get; set; } = default!;

  [ValidateNever]
  public ChartPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;

  [ValidateNever]
  public IEnumerable<ContentTypeDefinition> ChartContentTypes { get; set; } =
    default!;
}
