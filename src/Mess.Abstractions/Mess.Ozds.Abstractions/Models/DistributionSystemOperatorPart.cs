using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class DistributionSystemOperatorPart : ContentPart
{
  public ContentPickerField RegulatoryCatalogue { get; set; } = default!;

  public ContentPickerField OperatorCatalogue { get; set; } = default!;
}
