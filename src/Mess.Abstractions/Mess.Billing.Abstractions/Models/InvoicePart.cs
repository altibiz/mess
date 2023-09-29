using Mess.Billing.Abstractions.Invoices;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class InvoicePart : ContentPart
{
  public Invoice Invoice { get; set; } = default!;
}
