using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class OzdsIotDeviceDistributionSystemOperatorIndexProvider
  : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<OzdsIotDeviceDistributionSystemOperatorIndex>()
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

        return ozdsIotDevicePart.DistributionSystemOperatorRepresentativeUserIds.Select(
          closedDistributionSystemRepresentativeUserId =>
            new OzdsIotDeviceDistributionSystemOperatorIndex
            {
              ContentItemId = contentItem.ContentItemId,
              DeviceId = measurementDevicePart.DeviceId.Text,
              IsMessenger = measurementDevicePart.IsMessenger,
              DistributionSystemOperatorContentItemId =
                ozdsIotDevicePart.DistributionSystemOperatorContentItemId,
              DistributionSystemOperatorRepresentativeUserId =
                closedDistributionSystemRepresentativeUserId
            }
        );
      });
  }
}
