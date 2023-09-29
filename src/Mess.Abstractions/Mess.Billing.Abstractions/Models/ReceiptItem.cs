using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Billing.Abstractions.Models;

public class ReceiptItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<ReceiptPart> InvoicePart { get; private set; } = default!;

  private ReceiptItem(ContentItem contentItem)
    : base(contentItem) { }
}
