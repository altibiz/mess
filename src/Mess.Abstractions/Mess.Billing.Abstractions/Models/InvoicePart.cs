using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class InvoicePart : ContentPart {

  public ContentPickerField Receipt { get; set; } = default!;
 }
