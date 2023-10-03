using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class BillingPart : ContentPart
{
  public string LegalEntityContentItemId { get; set; } = default!;

  public string[] CatalogueContentItemIds { get; set; } = default!;
}
