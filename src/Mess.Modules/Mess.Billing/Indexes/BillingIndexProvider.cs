using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using Mess.Billing.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.Data;
using YesSql.Indexes;

namespace Mess.Billing.Indexes;

public class BillingIndexProvider
  : IndexProvider<ContentItem>,
    IScopedIndexProvider
{
  public override void Describe(DescribeContext<ContentItem> context) =>
    context
      .For<BillingIndex>()
      .When(contentItem => contentItem.Has<BillingPart>())
      .Map(async contentItem =>
      {
        var indexer = _serviceProvider
          .GetServices<IBillingIndexer>()
.FirstOrDefault(indexer => indexer.IsApplicable(contentItem));
        if (indexer is null)
        {
          _logger.LogError(
            "No billing indexer found for content item {} of type {}",
            contentItem.ContentItemId,
            contentItem.ContentType

          );
          return Array.Empty<BillingIndex>();
        }

        return new BillingIndex[]
        {
          await indexer.IndexBillingAsync(contentItem)
        };
      });

  public BillingIndexProvider(
    IServiceProvider serviceProvider,
    ILogger<BillingIndexProvider> logger
  )
  {
    _serviceProvider = serviceProvider;
    _logger = logger;
  }

  private readonly IServiceProvider _serviceProvider;

  private readonly ILogger _logger;
}
