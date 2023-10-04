using Mess.Billing.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsReceiptItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<ReceiptPart> ReceiptPart { get; private set; } = default!;

  public Lazy<OzdsCalculationPart> OzdsCalculationPart { get; private set; } =
    default!;

  public Lazy<OzdsReceiptPart> OzdsReceiptPart { get; private set; } = default!;

  private OzdsReceiptItem(ContentItem contentItem)
    : base(contentItem) { }
}
