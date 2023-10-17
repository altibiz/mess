using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class OzdsIotDeviceClosedDistributionSystemIndexProvider
  : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<OzdsIotDeviceClosedDistributionSystemIndex>()
      .When(
        contentItem =>
          contentItem.Has<OzdsIotDevicePart>()
          && contentItem.Has<IotDevicePart>()
      )
      .Map(contentItem =>
      {
        var measurementDevicePart = contentItem.As<IotDevicePart>();
        var ozdsIotDevicePart =
          contentItem.As<OzdsIotDevicePart>();

        return ozdsIotDevicePart.ClosedDistributionSystemRepresentativeUserIds.Select(
          closedDistributionSystemRepresentativeUserId =>
            new OzdsIotDeviceClosedDistributionSystemIndex
            {
              ContentItemId = contentItem.ContentItemId,
              DeviceId = measurementDevicePart.DeviceId.Text,
              IsMessenger = measurementDevicePart.IsMessenger,
              ClosedDistributionSystemContentItemId =
                ozdsIotDevicePart.ClosedDistributionSystemContentItemId,
              ClosedDistributionSystemRepresentativeUserId =
                closedDistributionSystemRepresentativeUserId
            }
        );
      });
  }
}
