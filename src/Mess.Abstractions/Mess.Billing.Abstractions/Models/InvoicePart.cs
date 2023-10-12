using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class InvoicePart : ContentPart
{
  public string BillingContentItemId { get; set; } = default!;

  public string IssuerContentItemId { get; set; } = default!;

  public string[] IssuerRepresentativeUserIds { get; set; } = default!;

  public string RecipientContentItemId { get; set; } = default!;

  public string[] RecipientRepresentativeUserIds { get; set; } = default!;

  public string? ReceiptContentItemId { get; set; } = default!;
}
