using Mess.ContentFields.Abstractions.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.ContentFields.ViewModels;

public class ApiKeyFieldEditViewModel
{
  public ApiKeyField Field { get; set; } = default!;

  public ContentPart Part { get; set; } = default!;

  public ContentPartFieldDefinition PartFieldDefinition { get; set; } =
    default!;

  public string Value { get; set; } = default!;
}