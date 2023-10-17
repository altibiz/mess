using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class ReceiptPart : ContentPart
{
  public ContentPickerField Invoice { get; set; } = default!;
}
