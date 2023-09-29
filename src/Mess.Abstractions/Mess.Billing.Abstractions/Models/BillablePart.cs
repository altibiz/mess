using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class BillablePart : ContentPart
{
  public ContentPickerField LegalEntity { get; set; } = default!;
}
