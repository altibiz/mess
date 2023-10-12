using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class ReceiptPart : ContentPart
{
  public string BillingContentItemId { get; set; } = default!;

  public string IssuerContentItemId { get; set; } = default!;

  public string[] IssuerRepresentativeUserIds { get; set; } = default!;

  public string RecipientContentItemId { get; set; } = default!;

  public string[] RecipientRepresentativeUserIds { get; set; } = default!;

  public string InvoiceContentItemId { get; set; } = default!;
}
