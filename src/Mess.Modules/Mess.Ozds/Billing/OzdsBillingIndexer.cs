using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Services;
using Mess.Cms;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;

namespace Mess.Ozds.Billing;

public class OzdsBillingIndexer : IBillingIndexer
{
  private readonly IContentManager _contentManager;

  public OzdsBillingIndexer(IContentManager contentManager)
  {
    _contentManager = contentManager;
  }

  public bool IsApplicable(ContentItem contentItem)
  {
    return contentItem.ContentType == "DistributionSystemUnit";
  }

  public BillingIndex IndexBilling(ContentItem contentItem)
  {
    var distributionSystemUnitItem =
      contentItem.AsContent<DistributionSystemUnitItem>();
    var closedDistributionSystemContentItemId = (
      distributionSystemUnitItem.ContainedPart.Value
      ?? throw new InvalidOperationException(
        $"Distribution system unit with content item id '{distributionSystemUnitItem.ContentItemId}' is not contained in a closed distribution system."
      )
    ).ListContentItemId;

    var closedDistributionSystemItem =
      _contentManager
        .GetContentAsync<ClosedDistributionSystemItem>(
          closedDistributionSystemContentItemId
        )
        .Result
      ?? throw new InvalidOperationException(
        $"Closed distribution system with content item id '{closedDistributionSystemContentItemId}' not found."
      );
    var distributionSystemOperatorContentItemId = (
      closedDistributionSystemItem.ContainedPart.Value
      ?? throw new InvalidOperationException(
        $"Closed distribution system with content item id '{closedDistributionSystemContentItemId}' is not contained in a distribution system operator."
      )
    ).ListContentItemId;

    return new BillingIndex
    {
      ContentItemId = contentItem.ContentItemId,
      ContentType = contentItem.ContentType,
      IssuerContentItemId = distributionSystemOperatorContentItemId,
      RecipientContentItemId = distributionSystemUnitItem.ContentItemId
    };
  }

  public async Task<BillingIndex> IndexBillingAsync(ContentItem contentItem)
  {
    var distributionSystemUnitItem =
      contentItem.AsContent<DistributionSystemUnitItem>();
    var closedDistributionSystemContentItemId = (
      distributionSystemUnitItem.ContainedPart.Value
      ?? throw new InvalidOperationException(
        $"Distribution system unit with content item id '{distributionSystemUnitItem.ContentItemId}' is not contained in a closed distribution system."
      )
    ).ListContentItemId;

    var closedDistributionSystemItem =
      await _contentManager.GetContentAsync<ClosedDistributionSystemItem>(
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
      await _contentManager.GetContentAsync<DistributionSystemOperatorItem>(
        distributionSystemOperatorContentItemId
      )
      ?? throw new InvalidOperationException(
        $"Distribution system operator with content item id '{distributionSystemOperatorContentItemId}' not found."
      );

    return new BillingIndex
    {
      ContentItemId = contentItem.ContentItemId,
      ContentType = contentItem.ContentType,
      IssuerContentItemId = distributionSystemOperatorContentItemId,
      RecipientContentItemId = distributionSystemUnitItem.ContentItemId
    };
  }
}
