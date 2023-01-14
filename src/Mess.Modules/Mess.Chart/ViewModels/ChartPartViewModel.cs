using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using Mess.Chart.Models;

namespace Mess.Chart.ViewModels;

public class ChartPartViewModel
{
  public string Type { get; set; } = default!;

  [BindNever]
  public ContentItem ContentItem { get; set; } = default!;

  [BindNever]
  public ChartPart ChartPart { get; set; } = default!;

  [BindNever]
  public ContentTypePartDefinition TypePartDefinition { get; set; } = default!;
}
