using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class ClosedDistributionSystemPart : ContentPart
{
  public ContentPickerField DistributionSystemOperator { get; set; } = default!;

  public ContentPickerField WhiteHighVoltageOperatorCatalogue { get; set; } =
    default!;

  public ContentPickerField WhiteMediumVoltageOperatorCatalogue { get; set; } =
    default!;

  public ContentPickerField BlueOperatorCatalogue { get; set; } = default!;

  public ContentPickerField WhiteLowVoltageOperatorCatalogue { get; set; } =
    default!;

  public ContentPickerField RedOperatorCatalogue { get; set; } = default!;

  public ContentPickerField YellowOperatorCatalogue { get; set; } = default!;
}
