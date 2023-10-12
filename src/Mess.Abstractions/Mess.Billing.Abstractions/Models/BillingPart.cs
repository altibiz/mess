using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class BillingPart : ContentPart
{
  public string IssuerContentItemId { get; set; } = default!;

  public string[] IssuerRepresentativeUserIds { get; set; } = default!;

  public string RecipientContentItemId { get; set; } = default!;

  public string[] RecipientRepresentativeUserIds { get; set; } = default!;
}
