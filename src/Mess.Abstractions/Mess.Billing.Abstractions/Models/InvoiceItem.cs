using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Billing.Abstractions.Models;

public class InvoiceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<InvoicePart> InvoicePart { get; private set; } = default!;

  private InvoiceItem(ContentItem contentItem)
    : base(contentItem) { }
}
