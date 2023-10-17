using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class OzdsMeasurementDeviceClosedDistributionSystemIndexProvider
  : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<OzdsMeasurementDeviceClosedDistributionSystemIndex>()
      .When(
        contentItem =>
          contentItem.Has<OzdsMeasurementDevicePart>()
          && contentItem.Has<IotDevicePart>()
      )
      .Map(contentItem =>
      {
        var measurementDevicePart = contentItem.As<IotDevicePart>();
        var ozdsMeasurementDevicePart =
          contentItem.As<OzdsMeasurementDevicePart>();

        return ozdsMeasurementDevicePart.ClosedDistributionSystemRepresentativeUserIds.Select(
          closedDistributionSystemRepresentativeUserId =>
            new OzdsMeasurementDeviceClosedDistributionSystemIndex
            {
              ContentItemId = contentItem.ContentItemId,
              DeviceId = measurementDevicePart.DeviceId.Text,
              IsMessenger = measurementDevicePart.IsMessenger,
              ClosedDistributionSystemContentItemId =
                ozdsMeasurementDevicePart.ClosedDistributionSystemContentItemId,
              ClosedDistributionSystemRepresentativeUserId =
                closedDistributionSystemRepresentativeUserId
            }
        );
      });
  }
}
