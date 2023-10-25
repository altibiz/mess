using Mess.OrchardCore;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using YesSql.Indexes;
using OrchardCore.Data;

namespace Mess.Ozds.Indexes;

public class DistributionSystemUnitIndexProvider
  : IndexProvider<ContentItem>,
    IScopedIndexProvider
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<DistributionSystemUnitIndex>()
      .When(contentItem => contentItem.Has<DistributionSystemUnitPart>())
      .Map(async contentItem =>
      {
        var contentManager =
          _serviceProvider.GetRequiredService<IContentManager>();
        var distributionSystemUnitPart =
          contentItem.As<DistributionSystemUnitPart>();
        var containedPart =
          contentItem.As<ContainedPart>()
          ?? throw new InvalidOperationException(
            $"Distribution system unit '{contentItem.ContentItemId}' does not have a '{nameof(ContainedPart)}'."
          );

        var closedDistributionSystemContentItemId =
          containedPart.ListContentItemId;

        var closedDistributionSystemItem =
          await contentManager.GetContentAsync<ClosedDistributionSystemItem>(
            closedDistributionSystemContentItemId
          )
          ?? throw new InvalidOperationException(
            $"Closed distribution system with content item id '{closedDistributionSystemContentItemId}' not found."
          );
        var distributionSystemOperatorContentItemId = (
          closedDistributionSystemItem.ContainedPart.Value
          ?? throw new InvalidOperationException(
            $"Closed distribution system with content item id '{closedDistributionSystemContentItemId}' is not contained in a distribution system operator."
          )
        ).ListContentItemId;

        var distributionSystemOperator =
          await contentManager.GetContentAsync<DistributionSystemOperatorItem>(
            distributionSystemOperatorContentItemId
          )
          ?? throw new InvalidOperationException(
            $"Distribution system operator with content item id '{distributionSystemOperatorContentItemId}' not found."
          );

        return new DistributionSystemUnitIndex
        {
          DistributionSystemUnitContentItemId = contentItem.ContentItemId,
          ClosedDistributionSystemContentItemId =
            closedDistributionSystemItem.ContentItemId,
          DistributionSystemOperatorContentItemId =
            distributionSystemOperatorContentItemId
        };
      });
  }

  public DistributionSystemUnitIndexProvider(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}
