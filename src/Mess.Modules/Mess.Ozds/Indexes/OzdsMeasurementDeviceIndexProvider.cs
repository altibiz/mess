using Mess.MeasurementDevice.Abstractions.Indexes;
using Mess.MeasurementDevice.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Ozds.Indexes;

public class OzdsMeasurementDeviceIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<MeasurementDeviceIndex>()
      .When(contentItem => contentItem.Has<MeasurementDevicePart>())
      .Map(contentItem =>
      {
        var measurementDevicePart = contentItem.As<MeasurementDevicePart>();

        return new MeasurementDeviceIndex
        {
          ContentItemId = contentItem.ContentItemId,
          DeviceId = measurementDevicePart.DeviceId.Text,
        };
      });
  }
}
