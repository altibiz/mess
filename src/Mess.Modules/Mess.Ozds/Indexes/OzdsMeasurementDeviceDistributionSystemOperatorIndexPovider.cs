using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class OzdsMeasurementDeviceDistributionSystemOperatorIndexProvider
  : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<OzdsMeasurementDeviceDistributionSystemOperatorIndex>()
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

        return ozdsMeasurementDevicePart.DistributionSystemOperatorRepresentativeUserIds.Select(
          closedDistributionSystemRepresentativeUserId =>
            new OzdsMeasurementDeviceDistributionSystemOperatorIndex
            {
              ContentItemId = contentItem.ContentItemId,
              DeviceId = measurementDevicePart.DeviceId.Text,
              IsMessenger = measurementDevicePart.IsMessenger,
              DistributionSystemOperatorContentItemId =
                ozdsMeasurementDevicePart.DistributionSystemOperatorContentItemId,
              DistributionSystemOperatorRepresentativeUserId =
                closedDistributionSystemRepresentativeUserId
            }
        );
      });
  }
}
