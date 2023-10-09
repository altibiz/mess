using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class InvoicePart : ContentPart
{
  public string BillingContentItemId { get; set; } = default!;

  public string LegalEntityContentItemId { get; set; } = default!;

  public string? ReceiptContentItemId { get; set; } = default!;
}
