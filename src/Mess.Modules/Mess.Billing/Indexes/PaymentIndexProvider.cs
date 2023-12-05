using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using Mess.Billing.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.Data;
using YesSql.Indexes;

namespace Mess.Billing.Indexes;

public class PaymentIndexProvider
  : IndexProvider<ContentItem>,
    IScopedIndexProvider
{
  private readonly ILogger _logger;

  private readonly IServiceProvider _serviceProvider;

  public PaymentIndexProvider(
    IServiceProvider serviceProvider,
    ILogger<PaymentIndexProvider> logger
  )
  {
    _serviceProvider = serviceProvider;
    _logger = logger;
  }

  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<PaymentIndex>()
      .When(
        contentItem =>
          contentItem.Has<InvoicePart>() || contentItem.Has<ReceiptPart>()
      )
      .Map(async contentItem =>
      {
        var indexer = _serviceProvider
          .GetServices<IPaymentIndexer>()
          .FirstOrDefault(indexer => indexer.IsApplicable(contentItem));
        if (indexer is null)
        {
          _logger.LogError(
            "No payment indexer found for content item {} of type {}",
            contentItem.ContentItemId,
            contentItem.ContentType
          );
          return Array.Empty<PaymentIndex>();
        }

        return new[]
        {
          await indexer.IndexPaymentAsync(contentItem)
        };
      });
  }
}
