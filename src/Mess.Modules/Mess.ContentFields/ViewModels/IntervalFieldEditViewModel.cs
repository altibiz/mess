using Mess.ContentFields.Abstractions;
using Mess.ContentFields.Abstractions.Fields;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.ContentFields.ViewModels;

public class IntervalFieldEditViewModel
{
  [ValidateNever]
  public IntervalField Field { get; set; } = default!;

  [ValidateNever]
  public ContentPart Part { get; set; } = default!;

  [ValidateNever]
  public ContentPartFieldDefinition PartFieldDefinition { get; set; } =
    default!;

  [ValidateNever]
  public List<SelectListItem> UnitOptions { get; set; } = default!;

  public Interval Value { get; set; } = default!;
}
