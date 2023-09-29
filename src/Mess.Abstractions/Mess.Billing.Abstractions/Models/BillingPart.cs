using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class BillingPart : ContentPart
{
  public ContentPickerField LegalEntity { get; set; } = default!;
}
