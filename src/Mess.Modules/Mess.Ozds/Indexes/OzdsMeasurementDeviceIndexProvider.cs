using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class OzdsMeasurementDeviceIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<OzdsMeasurementDeviceIndex>()
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

        return new OzdsMeasurementDeviceIndex
        {
          ContentItemId = contentItem.ContentItemId,
          DeviceId = measurementDevicePart.DeviceId.Text,
          DistributionSystemUnitContentItemId =
            ozdsMeasurementDevicePart.DistributionSystemUnitContentItemId,
          DistributionSystemUnitRepresentativeUserIds =
            ozdsMeasurementDevicePart.DistributionSystemUnitRepresentativeUserIds,
          ClosedDistributionSystemContentItemId =
            ozdsMeasurementDevicePart.ClosedDistributionSystemContentItemId,
          ClosedDistributionSystemRepresentativeUserIds =
            ozdsMeasurementDevicePart.ClosedDistributionSystemRepresentativeUserIds,
          DistributionSystemOperatorContentItemId =
            ozdsMeasurementDevicePart.DistributionSystemOperatorContentItemId,
          DistributionSystemOperatorRepresentativeUserIds =
            ozdsMeasurementDevicePart.DistributionSystemOperatorRepresentativeUserIds,
        };
      });
  }
}
