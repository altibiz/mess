using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsMeasurementDevicePart : ContentPart
{
  public ContentPickerField DistributionSystemUnit { get; set; } = default!;

  public ContentPickerField Catalogue { get; set; } = default!;

  public string ClosedDistributionSystemContentItemId { get; set; } = default!;

  public string[] ClosedDistributionSystemRepresentativeUserIds { get; set; } =
    default!;

  public string DistributionSystemOperatorContentItemId { get; set; } =
    default!;

  public string[] DistributionSystemOperatorRepresentativeUserIds { get; set; } =
    default!;

  public string DistributionSystemUnitContentItemId { get; set; } = default!;

  public string[] DistributionSystemUnitRepresentativeUserIds { get; set; } =
    default!;

  public string RegulatoryAgencyCatalogueContentItemId { get; set; } = default!;

  public string OperatorCatalogueContentItemId { get; set; } = default!;
}
