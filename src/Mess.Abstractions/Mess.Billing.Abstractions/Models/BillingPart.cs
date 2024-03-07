using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class BillingPart : ContentPart
{
  public DateTimeOffset LastInvoiceCreated { get; set; } = DateTimeOffset.MinValue.ToUniversalTime();

  public DateTimeOffset LastInvoiceFrom { get; set; } = DateTimeOffset.MinValue.ToUniversalTime();

  public DateTimeOffset LastInvoiceTo { get; set; } = DateTimeOffset.MinValue.ToUniversalTime();
}
