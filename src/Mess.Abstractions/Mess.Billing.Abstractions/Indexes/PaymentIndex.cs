using YesSql.Indexes;

namespace Mess.Billing.Abstractions.Indexes;

public class PaymentIndex : MapIndex
{
  public string ContentItemId { get; set; } = default!;

  public string ContentType { get; set; } = default!;

  public string BillingContentItemId { get; set; } = default!;

  public string InvoiceContentItemId { get; set; } = default!;

  public string? ReceiptContentItemId { get; set; } = default!;

  public string IssuerContentItemId { get; set; } = default!;

  public string RecipientContentItemId { get; set; } = default!;
}
