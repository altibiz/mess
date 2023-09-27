using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class OzdsMeasurementDeviceDistributionSystemUnitIndexProvider
  : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<OzdsMeasurementDeviceDistributionSystemUnitIndex>()
      .When(
        contentItem =>
          contentItem.Has<OzdsMeasurementDevicePart>()
          && contentItem.Has<MeasurementDevicePart>()
      )
      .Map(contentItem =>
      {
        var measurementDevicePart = contentItem.As<MeasurementDevicePart>();
        var ozdsMeasurementDevicePart =
          contentItem.As<OzdsMeasurementDevicePart>();

        return ozdsMeasurementDevicePart.DistributionSystemUnitRepresentativeUserIds.Select(
          closedDistributionSystemRepresentativeUserId =>
            new OzdsMeasurementDeviceDistributionSystemUnitIndex
            {
              ContentItemId = contentItem.ContentItemId,
              DeviceId = measurementDevicePart.DeviceId.Text,
              IsMessenger = measurementDevicePart.IsMessenger,
              DistributionSystemUnitContentItemId =
                ozdsMeasurementDevicePart.DistributionSystemUnitContentItemId,
              DistributionSystemUnitRepresentativeUserId =
                closedDistributionSystemRepresentativeUserId
            }
        );
      });
  }
}
