using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.OrchardCore.Extensions.Microsoft;
using Mess.OrchardCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using YesSql.Indexes;
using OrchardCore.Lists.Models;
using OrchardCore.Data;

namespace Mess.Ozds.Indexes;

public class OzdsIotDeviceIndexProvider
  : IndexProvider<ContentItem>,
    IScopedIndexProvider
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
        var contentManager =
          _serviceProvider.GetRequiredService<IContentManager>();

        var iotDevicePart = contentItem.As<IotDevicePart>();
        var ozdsIotDevicePart = contentItem.As<OzdsIotDevicePart>();
        var containedPart =
          contentItem.As<ContainedPart>()
          ?? throw new InvalidOperationException(
            $"OZDS IoT device '{contentItem.ContentItemId}' does not have a '{nameof(ContainedPart)}'."
          );

        var distributionSystemUnitContentItemId =
          containedPart.ListContentItemId;

        var distributionSystemUnitItem =
          await contentManager.GetContentAsync<DistributionSystemUnitItem>(
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
