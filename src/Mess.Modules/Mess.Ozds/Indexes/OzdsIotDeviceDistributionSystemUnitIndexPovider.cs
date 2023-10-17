using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class OzdsIotDeviceDistributionSystemUnitIndexProvider
  : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<OzdsIotDeviceDistributionSystemUnitIndex>()
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

        return ozdsIotDevicePart.DistributionSystemUnitRepresentativeUserIds.Select(
          closedDistributionSystemRepresentativeUserId =>
            new OzdsIotDeviceDistributionSystemUnitIndex
            {
              ContentItemId = contentItem.ContentItemId,
              DeviceId = measurementDevicePart.DeviceId.Text,
              IsMessenger = measurementDevicePart.IsMessenger,
              DistributionSystemUnitContentItemId =
                ozdsIotDevicePart.DistributionSystemUnitContentItemId,
              DistributionSystemUnitRepresentativeUserId =
                closedDistributionSystemRepresentativeUserId
            }
        );
      });
  }
}
