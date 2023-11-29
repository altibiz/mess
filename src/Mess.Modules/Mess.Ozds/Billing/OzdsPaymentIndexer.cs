using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using Mess.Billing.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Billing;

public class OzdsPaymentIndexer : IPaymentIndexer
{
  public bool IsApplicable(ContentItem contentItem)
  {
    return contentItem.Has<OzdsInvoicePart>()
      || contentItem.Has<OzdsReceiptPart>();
  }

  public PaymentIndex IndexPayment(ContentItem contentItem)
  {
    var ozdsCalculationPart = contentItem.As<OzdsCalculationPart>();

    var ozdsInvoicePart = contentItem.As<OzdsInvoicePart>();
    var invoicePart = contentItem.As<InvoicePart>();
    if (ozdsInvoicePart is not null && invoicePart is not null)
    {
      return new PaymentIndex
      {
        ContentItemId = contentItem.ContentItemId,
        ContentType = contentItem.ContentType,
        BillingContentItemId = ozdsCalculationPart.IotDevice.ContentItemId,
        InvoiceContentItemId = contentItem.ContentItemId,
        ReceiptContentItemId =
          invoicePart.Receipt.ContentItemIds.FirstOrDefault(),
        IssuerContentItemId = ozdsInvoicePart
          .DistributionSystemOperator
          .ContentItemId,
        RecipientContentItemId = ozdsInvoicePart
          .DistributionSystemUnit
          .ContentItemId,
      };
    }

    var ozdsReceiptPart = contentItem.As<OzdsReceiptPart>();
    var receiptPart = contentItem.As<ReceiptPart>();

    return ozdsReceiptPart is not null && receiptPart is not null
      ? new PaymentIndex
      {
        ContentItemId = contentItem.ContentItemId,
        ContentType = contentItem.ContentType,
        BillingContentItemId = ozdsCalculationPart.IotDevice.ContentItemId,
        InvoiceContentItemId = receiptPart.Invoice.ContentItemIds.First(),
        ReceiptContentItemId = contentItem.ContentItemId,
        IssuerContentItemId = ozdsReceiptPart
          .DistributionSystemOperator
          .ContentItemId,
        RecipientContentItemId = ozdsReceiptPart
          .DistributionSystemUnit
          .ContentItemId,
      }
      : throw new InvalidOperationException(
      $"Payment with content item id '{contentItem.ContentItemId}' not found."
    );
  }

  public async Task<PaymentIndex> IndexPaymentAsync(ContentItem contentItem)
  {
    return IndexPayment(contentItem);
  }
}
