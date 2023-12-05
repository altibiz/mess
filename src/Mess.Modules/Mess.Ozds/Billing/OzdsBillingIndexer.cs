using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;
using Mess.Cms;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;

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
    var containedPart =
      contentItem.As<ContainedPart>()
      ?? throw new InvalidOperationException(
        $"OZDS IoT device '{contentItem.ContentItemId}' does not have a '{nameof(ContainedPart)}'."
      );

    var distributionSystemUnitContentItemId = containedPart.ListContentItemId;

    var distributionSystemUnitItem =
      _contentManager
        .GetContentAsync<DistributionSystemUnitItem>(
          distributionSystemUnitContentItemId
        )
        .Result
      ?? throw new InvalidOperationException(
        $"Distribution system unit with content item id '{distributionSystemUnitContentItemId}' not found."
      );
    var closedDistributionSystemContentItemId = (
      distributionSystemUnitItem.ContainedPart.Value
      ?? throw new InvalidOperationException(
        $"Distribution system unit with content item id '{distributionSystemUnitContentItemId}' is not contained in a closed distribution system."
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
      RecipientContentItemId = distributionSystemUnitContentItemId
    };
  }

  public async Task<BillingIndex> IndexBillingAsync(ContentItem contentItem)
  {
    var ozdsIotDevicePart = contentItem.As<OzdsIotDevicePart>();
    var containedPart =
      contentItem.As<ContainedPart>()
      ?? throw new InvalidOperationException(
        $"OZDS IoT device '{contentItem.ContentItemId}' does not have a '{nameof(ContainedPart)}'."
      );

    var distributionSystemUnitContentItemId = containedPart.ListContentItemId;

    var distributionSystemUnitItem =
      await _contentManager.GetContentAsync<DistributionSystemUnitItem>(
        distributionSystemUnitContentItemId
      )
      ?? throw new InvalidOperationException(
        $"Distribution system unit with content item id '{distributionSystemUnitContentItemId}' not found."
      );
    var closedDistributionSystemContentItemId = (
      distributionSystemUnitItem.ContainedPart.Value
      ?? throw new InvalidOperationException(
        $"Distribution system unit with content item id '{distributionSystemUnitContentItemId}' is not contained in a closed distribution system."
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
      RecipientContentItemId = distributionSystemUnitContentItemId
    };
  }

  public OzdsBillingIndexer(IContentManager contentManager)
  {
    _contentManager = contentManager;
  }

  private readonly IContentManager _contentManager;
}
