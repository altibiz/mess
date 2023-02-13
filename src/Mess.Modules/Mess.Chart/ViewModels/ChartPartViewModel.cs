using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement.Metadata.Models;
using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Chart.ViewModels;

public class ChartPartViewModel
{
  public List<SelectListItem> ProviderIds { get; set; } = default!;

  public ChartPart Part { get; set; } = default!;

  [BindNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;

  [BindNever]
  public IEnumerable<ContentTypeDefinition> ChartContentTypes { get; set; } =
    default!;
}
