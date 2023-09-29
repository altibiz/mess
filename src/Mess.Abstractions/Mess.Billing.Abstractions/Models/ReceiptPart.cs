using Mess.Billing.Abstractions.Receipts;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class ReceiptPart : ContentPart
{
  public Receipt Invoice { get; set; } = default!;
}
