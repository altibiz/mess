using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsReceiptPart : ContentPart
{
  public ContentItem DistributionSystemOperator { get; set; } = default!;

  public ContentItem ClosedDistributionSystem { get; set; } = default!;

  public ContentItem DistributionSystemUnit { get; set; } = default!;

  public OzdsReceiptData Data { get; set; } = default!;
}
