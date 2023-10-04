using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class BillingPart : ContentPart
{
  public string LegalEntityContentItemId { get; set; } = default!;
}
