using Mess.Iot.Abstractions.Indexes;
using Mess.Iot.Abstractions.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Iot.Indexes;

public class IotDeviceIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<IotDeviceIndex>()
      .When(contentItem => contentItem.Has<IotDevicePart>())
      .Map(contentItem =>
      {
        var measurementDevicePart = contentItem.As<IotDevicePart>();

        return new IotDeviceIndex
        {
          ContentItemId = contentItem.ContentItemId,
          DeviceId = measurementDevicePart.DeviceId.Text,
          IsMessenger = measurementDevicePart.IsMessenger
        };
      });
  }
}
