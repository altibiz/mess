using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsCalculationPart : ContentPart
{
  public ContentItem IotDevice { get; set; } = default!;

  public ContentItem RegulatoryAgencyCatalogue { get; set; } = default!;

  public ContentItem UsageCatalogue { get; set; } = default!;

  public ContentItem SupplyCatalogue { get; set; } = default!;

  public OzdsCalculationData Data { get; set; } = default!;
}
