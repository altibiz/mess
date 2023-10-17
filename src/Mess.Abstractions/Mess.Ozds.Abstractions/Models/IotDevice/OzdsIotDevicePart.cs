using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsIotDevicePart : ContentPart
{
  public ContentPickerField DistributionSystemUnit { get; set; } = default!;

  public ContentPickerField UsageCatalogue { get; set; } = default!;

  public ContentPickerField SupplyCatalogue { get; set; } = default!;
}
