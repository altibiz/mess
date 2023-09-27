using Mess.Iot.Abstractions.Indexes;
using Mess.Iot.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Iot.Indexes;

public class MeasurementDeviceIndexProvider : IndexProvider<ContentItem>
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
          IsMessenger = measurementDevicePart.IsMessenger,
        };
      });
  }
}
