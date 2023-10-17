using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.System.Extensions.Microsoft;
using Mess.OrchardCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class OzdsIotDeviceIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<OzdsIotDeviceIndex>()
      .When(
        contentItem =>
          contentItem.Has<OzdsIotDevicePart>()
          && contentItem.Has<IotDevicePart>()
      )
      .Map(async contentItem =>
      {
        var iotDevicePart = contentItem.As<IotDevicePart>();
        var ozdsIotDevicePart = contentItem.As<OzdsIotDevicePart>();

        var distributionSystemUnitContentItemId =
          ozdsIotDevicePart.DistributionSystemUnit.ContentItemIds.First();

        var distributionSystemUnitItem =
          await _serviceProvider.AwaitScopeAsync(async serviceProvider =>
          {
            var contentManager =
              serviceProvider.GetRequiredService<IContentManager>();
            return await contentManager.GetContentAsync<DistributionSystemUnitItem>(
              distributionSystemUnitContentItemId
            );
          })
          ?? throw new InvalidOperationException(
            $"Distribution system unit with content item id '{distributionSystemUnitContentItemId}' not found."
          );
        var closedDistributionSystemContentItemId =
          distributionSystemUnitItem.DistributionSystemUnitPart.Value.ClosedDistributionSystem.ContentItemIds.First();

        var closedDistributionSystemItem =
          await _serviceProvider.AwaitScopeAsync(async serviceProvider =>
          {
            var contentManager =
              serviceProvider.GetRequiredService<IContentManager>();
            return await contentManager.GetContentAsync<ClosedDistributionSystemItem>(
              closedDistributionSystemContentItemId
            );
          })
          ?? throw new InvalidOperationException(
            $"Closed distribution system with content item id '{closedDistributionSystemContentItemId}' not found."
          );
        var distributionSystemOperatorContentItemId =
          closedDistributionSystemItem.ClosedDistributionSystemPart.Value.DistributionSystemOperator.ContentItemIds.First();

        var distributionSystemOperator =
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

        return new OzdsIotDeviceIndex
        {
          OzdsIotDeviceContentItemId = contentItem.ContentItemId,
          DeviceId = iotDevicePart.DeviceId.Text,
          IsMessenger = iotDevicePart.IsMessenger,
          DistributionSystemUnitContentItemId =
            distributionSystemUnitContentItemId,
          ClosedDistributionSystemContentItemId =
            closedDistributionSystemContentItemId,
          DistributionSystemOperatorContentItemId =
            distributionSystemOperatorContentItemId
        };
      });
  }

  public OzdsIotDeviceIndexProvider(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}
