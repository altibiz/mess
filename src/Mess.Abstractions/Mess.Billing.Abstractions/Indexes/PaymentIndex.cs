using YesSql.Indexes;

namespace Mess.Billing.Abstractions.Indexes;

public class PaymentIndex : MapIndex
{
  public string InvoiceContentItemId { get; set; } = default!;

  public string? ReceiptContentItemId { get; set; } = default!;
}
