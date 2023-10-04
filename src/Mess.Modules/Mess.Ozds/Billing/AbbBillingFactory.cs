using Mess.Billing.Abstractions;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Billing;

public class AbbBillingFactory : IBillingFactory
{
  public const string BillingContentType = "AbbMeasurementDevice";

  public string ContentType => BillingContentType;

  public ContentItem CreateInvoice(ContentItem contentItem, ContentItem[] catalogueContentItems)
  {
    throw new NotImplementedException();
  }

  public Task<ContentItem> CreateInvoiceAsync(ContentItem contentItem, ContentItem[] catalogueContentItems)
  {
    throw new NotImplementedException();
  }

  public ContentItem CreateReceipt(ContentItem contentItem, ContentItem invoiceContentItem)
  {
    throw new NotImplementedException();
  }

  public Task<ContentItem> CreateReceiptAsync(ContentItem contentItem, ContentItem invoiceContentItem)
  {
    throw new NotImplementedException();
  }
}
