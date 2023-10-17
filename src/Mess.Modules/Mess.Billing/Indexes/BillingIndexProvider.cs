using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using Mess.Billing.Abstractions.Services;
using Mess.System.Extensions.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Billing.Indexes;

public class BillingIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context) =>
    context
      .For<BillingIndex>()
      .When(contentItem => contentItem.Has<BillingPart>())
      .Map(
        async contentItem =>
          await _serviceProvider.AwaitScopeAsync(async serviceProvider =>
          {
            var indexer = serviceProvider
              .GetServices<IBillingIndexer>()
              .Where(indexer => indexer.ContentType == contentItem.ContentType)
              .FirstOrDefault();
            if (indexer is null)
            {
              _logger.LogError(
                $"No billing indexer found for content item {contentItem.ContentItemId} of type {contentItem.ContentType}"
              );
              return Array.Empty<BillingIndex>();
            }

            return new BillingIndex[]
            {
              await indexer.IndexBillingAsync(contentItem)
            };
          })
      );

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
