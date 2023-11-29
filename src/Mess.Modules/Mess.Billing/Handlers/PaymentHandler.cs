using Mess.Billing.Abstractions.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;

// TODO: Send email

namespace Mess.Billing.Handlers;

public class PaymentHandler : ContentHandlerBase
{
  public override async Task CreatedAsync(CreateContentContext context)
  {
    var contentItem = context.ContentItem;

    if (contentItem.Has<ReceiptPart>())
    {
      var _ = contentItem.As<ReceiptPart>();
    }
    if (contentItem.Has<InvoicePart>())
    {
      var _ = contentItem.As<InvoicePart>();
    }
  }
}
