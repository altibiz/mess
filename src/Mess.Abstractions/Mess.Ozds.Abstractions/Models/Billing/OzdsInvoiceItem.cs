using Mess.Billing.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Title.Models;

namespace Mess.Ozds.Abstractions.Models;

public class OzdsInvoiceItem : ContentItemBase
{
  public Lazy<TitlePart> TitlePart { get; private set; } = default!;

  public Lazy<InvoicePart> InvoicePart { get; private set; } = default!;

  public Lazy<OzdsCalculationPart> OzdsCalculationPart { get; private set; } =
    default!;

  public Lazy<OzdsInvoicePart> OzdsInvoicePart { get; private set; } = default!;

  private OzdsInvoiceItem(ContentItem contentItem)
    : base(contentItem) { }
}
