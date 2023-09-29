using Mess.Billing.Abstractions.Receipts;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class ReceiptPart : ContentPart
{
  public Receipt Receipt { get; set; } = default!;
}
