using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsInvoicePart : ContentPart
{
  public ContentItem DistributionSystemOperator { get; set; } = default!;

  public ContentItem ClosedDistributionSystem { get; set; } = default!;

  public ContentItem DistributionSystemUnit { get; set; } = default!;

  public OzdsInvoiceData Data { get; set; } = default!;
}
