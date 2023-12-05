using Mess.Billing.Abstractions.Indexes;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Services;

public interface IPaymentIndexer
{
  public bool IsApplicable(ContentItem contentItem);

  public PaymentIndex IndexPayment(ContentItem contentItem);

  public Task<PaymentIndex> IndexPaymentAsync(ContentItem contentItem);
}
