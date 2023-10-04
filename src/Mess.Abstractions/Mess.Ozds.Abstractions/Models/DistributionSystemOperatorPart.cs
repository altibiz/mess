using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class DistributionSystemOperatorPart : ContentPart
{
  public ContentPickerField RegulatoryCatalogue { get; set; } = default!;

  // TODO: UI

  public ContentPickerField WhiteHighVoltageOperatorCatalogueContentItemId { get; set; } = default!;

  public ContentPickerField WhiteMediumVoltageOperatorCatalogueContentItemId { get; set; } = default!;

  public ContentPickerField BlueOperatorCatalogueContentItemId { get; set; } = default!;

  public ContentPickerField WhiteLowVoltageOperatorCatalogueContentItemId { get; set; } = default!;

  public ContentPickerField RedOperatorCatalogueContentItemId { get; set; } = default!;

  public ContentPickerField YellowOperatorCatalogueContentItemId { get; set; } = default!;
}
