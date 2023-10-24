using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using Mess.Billing.Abstractions.Services;
using Mess.System.Extensions.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Billing.Indexes;

public class PaymentIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context) =>
    context
      .For<PaymentIndex>()
      .When(
        contentItem =>
          contentItem.Has<InvoicePart>() || contentItem.Has<ReceiptPart>()
      )
      .Map(
        async contentItem =>
          await _serviceProvider.AwaitScopeAsync(async serviceProvider =>
          {
            var indexer = serviceProvider
              .GetServices<IPaymentIndexer>()
              .Where(indexer => indexer.IsApplicable(contentItem))
              .FirstOrDefault();
            if (indexer is null)
            {
              _logger.LogError(
                $"No payment indexer found for content item {contentItem.ContentItemId} of type {contentItem.ContentType}"
              );
              return Array.Empty<PaymentIndex>();
            }

            return new PaymentIndex[]
            {
              await indexer.IndexPaymentAsync(contentItem)
            };
          })
      );

  public PaymentIndexProvider(
    IServiceProvider serviceProvider,
    ILogger<PaymentIndexProvider> logger
  )
  {
    _serviceProvider = serviceProvider;
    _logger = logger;
  }

  private readonly IServiceProvider _serviceProvider;

  private readonly ILogger _logger;
}
