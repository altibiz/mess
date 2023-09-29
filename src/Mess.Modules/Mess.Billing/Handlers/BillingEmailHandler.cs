using Mess.Billing.Abstractions.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;

namespace Mess.Billing.Handler;

public class BillingSendHandler : ContentHandlerBase
{
  public override async Task CreatedAsync(CreateContentContext context)
  {
    var contentItem = context.ContentItem;

    if (contentItem.Has<ReceiptPart>())
    {
      var receiptPart = contentItem.As<ReceiptPart>();
      // TODO: Send email
    }
    if (contentItem.Has<InvoicePart>())
    {
      var invoicePart = contentItem.As<InvoicePart>();
      // TODO: Send email
    }
  }

}
