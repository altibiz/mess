using Mess.Chart.Abstractions.Models;
using Mess.Chart.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.Chart.ViewModels;

public class ChartFieldViewModel
{
  public ChartParameters Parameters { get; set; } = default!;

  [BindNever]
  public ChartField Field { get; set; } = default!;

  [BindNever]
  public ContentPart Part { get; set; } = default!;

  [BindNever]
  public ContentItem ContentItem { get; set; } = default!;

  [BindNever]
  public ContentPartFieldDefinition PartFieldDefinition { get; set; } =
    default!;
}
