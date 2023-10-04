using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class ReceiptPart : ContentPart
{
  public string BillingContentItemId { get; set; } = default!;

  public string LegalEntityContentItemId { get; set; } = default!;

  public string[] CatalogueContentItemIds { get; set; } = default!;

  public string InvoiceContentItemId { get; set; } = default!;
}
