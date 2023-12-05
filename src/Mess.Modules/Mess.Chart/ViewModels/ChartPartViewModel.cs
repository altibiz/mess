using Mess.Chart.Abstractions.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.Chart.ViewModels;

public class ChartPartViewModel
{
  [ValidateNever] public ChartPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;

  public string ChartContentItemId { get; set; } = default!;
}
