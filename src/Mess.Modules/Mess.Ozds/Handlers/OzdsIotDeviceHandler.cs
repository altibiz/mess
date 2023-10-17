using Mess.Billing.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.OrchardCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using YesSql;

namespace Mess.Ozds.Handlers;

public class OzdsIotDeviceHandler : ContentHandlerBase
{
  public override async Task CreatingAsync(CreateContentContext context)
  {
    await Update(context.ContentItem);
  }

  public override async Task UpdatingAsync(UpdateContentContext context)
  {
    await Update(context.ContentItem);
  }

  private async Task Update(ContentItem contentItem)
  {
    var contentManager = _serviceProvider.GetRequiredService<IContentManager>();
    var session = _serviceProvider.GetRequiredService<ISession>();

    var distributionSystemUnitContentItemId = contentItem
      .As<OzdsIotDevicePart>()
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

    contentItem.Alter<OzdsIotDevicePart>(ozdsIotDevicePart =>
    {
      ozdsIotDevicePart.ClosedDistributionSystemContentItemId =
        closedDistributionSystemContentItem.ContentItemId;
      ozdsIotDevicePart.ClosedDistributionSystemRepresentativeUserIds =
        closedDistributionSystemContentItem
          .LegalEntityPart
          .Value
          .Representatives
          .UserIds;

      ozdsIotDevicePart.DistributionSystemUnitContentItemId =
        distributionSystemUnitContentItem.ContentItemId;
      ozdsIotDevicePart.DistributionSystemUnitRepresentativeUserIds =
        distributionSystemUnitContentItem
          .LegalEntityPart
          .Value
          .Representatives
          .UserIds;

      ozdsIotDevicePart.DistributionSystemOperatorContentItemId =
        distributionSystemOperatorContentItem.ContentItemId;
      ozdsIotDevicePart.DistributionSystemOperatorRepresentativeUserIds =
        distributionSystemOperatorContentItem
          .LegalEntityPart
          .Value
          .Representatives
          .UserIds;
    });

    if (contentItem.Has<BillingPart>())
    {
      var ozdsIotDevicePart =
        contentItem.As<OzdsIotDevicePart>();
      contentItem.Alter<BillingPart>(billingPart =>
      {
        billingPart.IssuerContentItemId =
          distributionSystemOperatorContentItem.ContentItemId;
        billingPart.IssuerRepresentativeUserIds =
          distributionSystemOperatorContentItem
            .LegalEntityPart
            .Value
            .Representatives
            .UserIds;
        billingPart.RecipientContentItemId =
          distributionSystemUnitContentItem.ContentItemId;
        billingPart.RecipientRepresentativeUserIds =
          distributionSystemUnitContentItem
            .LegalEntityPart
            .Value
            .Representatives
            .UserIds;
      });
    }
  }

  public OzdsIotDeviceHandler(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}
