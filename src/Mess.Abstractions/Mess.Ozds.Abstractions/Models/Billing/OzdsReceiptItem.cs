using Mess.Billing.Abstractions.Models;
using Mess.Cms;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsReceiptItem : ContentItemBase
{
  private OzdsReceiptItem(ContentItem contentItem)
    : base(contentItem)
  {
  }

  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<ReceiptPart> ReceiptPart { get; private set; } = default!;

  public Lazy<OzdsCalculationPart> OzdsCalculationPart { get; private set; } =
    default!;

  public Lazy<OzdsReceiptPart> OzdsReceiptPart { get; private set; } = default!;
}
