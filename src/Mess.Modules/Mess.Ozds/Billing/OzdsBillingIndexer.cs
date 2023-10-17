using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Services;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Ozds.Billing;

public class OzdsBillingIndexer : IBillingIndexer
{
  public bool IsApplicable(ContentItem contentItem)
  {
    return contentItem.Has<OzdsIotDevicePart>();
  }

  public BillingIndex IndexBilling(ContentItem contentItem)
  {
    var ozdsIotDevicePart = contentItem.As<OzdsIotDevicePart>();

    var distributionSystemUnitContentItemId =
      ozdsIotDevicePart.DistributionSystemUnit.ContentItemIds.First();

    var distributionSystemUnitItem =
      _contentManager
        .GetContentAsync<DistributionSystemUnitItem>(
          distributionSystemUnitContentItemId
        )
        .Result
      ?? throw new InvalidOperationException(
        $"Distribution system unit with content item id '{distributionSystemUnitContentItemId}' not found."
      );
    var closedDistributionSystemContentItemId =
      distributionSystemUnitItem.DistributionSystemUnitPart.Value.ClosedDistributionSystem.ContentItemIds.First();

    var closedDistributionSystemItem =
      _contentManager
        .GetContentAsync<ClosedDistributionSystemItem>(
          closedDistributionSystemContentItemId
        )
        .Result
      ?? throw new InvalidOperationException(
        $"Closed distribution system with content item id '{closedDistributionSystemContentItemId}' not found."
      );
    var distributionSystemOperatorContentItemId =
      closedDistributionSystemItem.ClosedDistributionSystemPart.Value.DistributionSystemOperator.ContentItemIds.First();

    var distributionSystemOperator =
      _contentManager
        .GetContentAsync<DistributionSystemOperatorItem>(
          distributionSystemOperatorContentItemId
        )
        .Result
      ?? throw new InvalidOperationException(
        $"Distribution system operator with content item id '{distributionSystemOperatorContentItemId}' not found."
      );

    return new BillingIndex
    {
      ContentItemId = contentItem.ContentItemId,
      ContentType = contentItem.ContentType,
      IssuerContentItemId = distributionSystemOperatorContentItemId,
      RecipientContentItemId = distributionSystemUnitContentItemId
    };
  }

  public async Task<BillingIndex> IndexBillingAsync(ContentItem contentItem)
  {
    var ozdsIotDevicePart = contentItem.As<OzdsIotDevicePart>();

    var distributionSystemUnitContentItemId =
      ozdsIotDevicePart.DistributionSystemUnit.ContentItemIds.First();

    var distributionSystemUnitItem =
      await _contentManager.GetContentAsync<DistributionSystemUnitItem>(
        distributionSystemUnitContentItemId
      )
      ?? throw new InvalidOperationException(
        $"Distribution system unit with content item id '{distributionSystemUnitContentItemId}' not found."
      );
    var closedDistributionSystemContentItemId =
      distributionSystemUnitItem.DistributionSystemUnitPart.Value.ClosedDistributionSystem.ContentItemIds.First();

    var closedDistributionSystemItem =
      await _contentManager.GetContentAsync<ClosedDistributionSystemItem>(
        closedDistributionSystemContentItemId
      )
      ?? throw new InvalidOperationException(
        $"Closed distribution system with content item id '{closedDistributionSystemContentItemId}' not found."
      );
    var distributionSystemOperatorContentItemId =
      closedDistributionSystemItem.ClosedDistributionSystemPart.Value.DistributionSystemOperator.ContentItemIds.First();

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
      RecipientContentItemId = distributionSystemUnitContentItemId
    };
  }

  public OzdsBillingIndexer(IContentManager contentManager)
  {
    _contentManager = contentManager;
  }

  private readonly IContentManager _contentManager;
}
