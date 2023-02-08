using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement.Metadata.Models;
using Mess.Chart.Abstractions.Models;

namespace Mess.Chart.ViewModels;

public class ChartPartViewModel
{
  [BindNever]
  public ChartPart Part { get; set; } = default!;

  [BindNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;

  [BindNever]
  public IEnumerable<string> ProviderIds { get; set; } = default!;

  [BindNever]
  public IEnumerable<ContentTypeDefinition> ChartContentTypes { get; set; } =
    default!;
}
