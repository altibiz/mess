using Mess.Billing.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.OrchardCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using YesSql;

namespace Mess.Ozds.Handlers;

public class OzdsMeasurementDeviceHandler : ContentHandlerBase
{
  public override async Task CreatingAsync(CreateContentContext context)
  {
    var contentManager = _serviceProvider.GetRequiredService<IContentManager>();
    var session = _serviceProvider.GetRequiredService<ISession>();

    var distributionSystemUnitContentItemId = context.ContentItem
      .As<OzdsMeasurementDevicePart>()
      ?.DistributionSystemUnit.ContentItemIds.FirstOrDefault();
    if (distributionSystemUnitContentItemId == null)
    {
      return;
    }

    var unitIndex = await session
      .QueryIndex<DistributionSystemUnitIndex>()
      .Where(
        index =>
          index.DistributionSystemUnitContentItemId
          == distributionSystemUnitContentItemId
      )
      .FirstOrDefaultAsync();
    if (unitIndex == null)
    {
      return;
    }

    var closedDistributionSystemContentItem =
      await contentManager.GetContentAsync<ClosedDistributionSystemItem>(
        unitIndex.ClosedDistributionSystemContentItemId
      );
    if (closedDistributionSystemContentItem == null)
    {
      return;
    }

    var distributionSystemUnitContentItem =
      await contentManager.GetContentAsync<DistributionSystemUnitItem>(
        unitIndex.DistributionSystemUnitContentItemId
      );
    if (distributionSystemUnitContentItem == null)
    {
      return;
    }

    var distributionSystemOperatorContentItem =
      await contentManager.GetContentAsync<DistributionSystemOperatorItem>(
        unitIndex.DistributionSystemOperatorContentItemId
      );
    if (distributionSystemOperatorContentItem == null)
    {
      return;
    }

    context.ContentItem.Alter<OzdsMeasurementDevicePart>(ozdsMeasurementDevicePart =>
    {
      ozdsMeasurementDevicePart.ClosedDistributionSystemContentItemId =
        closedDistributionSystemContentItem.ContentItemId;
      ozdsMeasurementDevicePart.ClosedDistributionSystemRepresentativeUserIds =
        closedDistributionSystemContentItem
          .LegalEntityPart
          .Value
          .Representatives
          .UserIds;

      ozdsMeasurementDevicePart.DistributionSystemUnitContentItemId =
        distributionSystemUnitContentItem.ContentItemId;
      ozdsMeasurementDevicePart.DistributionSystemUnitRepresentativeUserIds =
        distributionSystemUnitContentItem
          .LegalEntityPart
          .Value
          .Representatives
          .UserIds;

      ozdsMeasurementDevicePart.DistributionSystemOperatorContentItemId =
        distributionSystemOperatorContentItem.ContentItemId;
      ozdsMeasurementDevicePart.DistributionSystemOperatorRepresentativeUserIds =
        distributionSystemOperatorContentItem
          .LegalEntityPart
          .Value
          .Representatives
          .UserIds;
    });

    if (context.ContentItem.Has<BillingPart>())
    {
      context.ContentItem.Alter<BillingPart>(billingPart =>
      {
        billingPart.LegalEntityContentItemId =
          distributionSystemUnitContentItemId;
      });
    }
  }

  public OzdsMeasurementDeviceHandler(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}