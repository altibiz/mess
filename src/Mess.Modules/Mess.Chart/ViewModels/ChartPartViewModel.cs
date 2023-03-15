using OrchardCore.ContentManagement.Metadata.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Mess.Chart.Abstractions.Models;

namespace Mess.Chart.ViewModels;

public class ChartPartViewModel
{
  [ValidateNever]
  public ChartPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;

  public string DataProviderId { get; set; } = default!;

  public string ChartContentItemId { get; set; } = default!;
}
