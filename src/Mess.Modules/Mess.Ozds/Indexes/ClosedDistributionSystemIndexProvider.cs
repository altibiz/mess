using Mess.OrchardCore;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.System.Extensions.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class ClosedDistributionSystemIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<ClosedDistributionSystemIndex>()
      .When(contentItem => contentItem.Has<ClosedDistributionSystemPart>())
      .Map(async contentItem =>
      {
        var distributionSystemUnitPart =
          contentItem.As<ClosedDistributionSystemPart>();

        var distributionSystemOperatorContentItemId =
          distributionSystemUnitPart.DistributionSystemOperator.ContentItemIds.First();

        var distributionSystemOperatorItem =
          await _serviceProvider.AwaitScopeAsync(async serviceProvider =>
          {
            var contentManager =
              serviceProvider.GetRequiredService<IContentManager>();
            return await contentManager.GetContentAsync<DistributionSystemOperatorItem>(
              distributionSystemOperatorContentItemId
            );
          })
          ?? throw new InvalidOperationException(
            $"Distribution system operator with content item id '{distributionSystemOperatorContentItemId}' not found."
          );

        return new ClosedDistributionSystemIndex[]
        {
          new ClosedDistributionSystemIndex
          {
            ClosedDistributionSystemContentItemId = contentItem.ContentItemId,
            DistributionSystemOperatorContentItemId =
              distributionSystemOperatorContentItemId
          }
        };
      });
  }

  public ClosedDistributionSystemIndexProvider(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}
