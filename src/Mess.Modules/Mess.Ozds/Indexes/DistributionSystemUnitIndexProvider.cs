using Mess.Iot.Abstractions.Models;
using Mess.OrchardCore;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class DistributionSystemUnitIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<DistributionSystemUnitIndex>()
      .When(contentItem => contentItem.Has<DistributionSystemUnitPart>())
      .Map(contentItem =>
      {
        var distributionSystemUnitPart =
          contentItem.As<DistributionSystemUnitPart>();

        var contentManager =
          _serviceProvider.GetRequiredService<IContentManager>();
        var closedDistributionSystemContentItem = contentManager
          .GetContentAsync<ClosedDistributionSystemItem>(
            distributionSystemUnitPart.ClosedDistributionSystem.ContentItemIds.First()
          )
          .Result;
        if (closedDistributionSystemContentItem == null)
        {
          return Array.Empty<DistributionSystemUnitIndex>();
        }

        return new DistributionSystemUnitIndex[]
        {
          new DistributionSystemUnitIndex
          {
            DistributionSystemUnitContentItemId = contentItem.ContentItemId,
            ClosedDistributionSystemContentItemId =
              closedDistributionSystemContentItem.ContentItemId,
            DistributionSystemOperatorContentItemId =
              closedDistributionSystemContentItem.ClosedDistributionSystemPart.Value.DistributionSystemOperator.ContentItemIds.First()
          }
        };
      });
  }

  public DistributionSystemUnitIndexProvider(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}
