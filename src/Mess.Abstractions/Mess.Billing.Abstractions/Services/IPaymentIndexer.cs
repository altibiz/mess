using OrchardCore.ContentManagement;
using Mess.Billing.Abstractions.Indexes;

namespace Mess.Billing.Abstractions.Services;

public interface IPaymentIndexer
{
  public string ContentType { get; }

  public PaymentIndex IndexPayment(ContentItem contentItem);

  public Task<PaymentIndex> IndexPaymentAsync(ContentItem contentItem);
}
