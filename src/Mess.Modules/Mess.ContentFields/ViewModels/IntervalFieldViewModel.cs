using Mess.ContentFields.Abstractions.Fields;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.ContentFields.ViewModels;

public class IntervalFieldViewModel
{
  [ValidateNever]
  public IntervalField Field { get; set; } = default!;

  [ValidateNever]
  public ContentPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentPartFieldDefinition PartFieldDefinition { get; set; } =
    default!;
}
