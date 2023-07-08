using Mess.MeasurementDevice.Abstractions.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.MeasurementDevice.ViewModels;

public class ApiKeyFieldViewModel
{
  public ApiKeyField Field { get; set; } = default!;

  public ContentPart Part { get; set; } = default!;

  public ContentPartFieldDefinition PartFieldDefinition { get; set; } =
    default!;
}
