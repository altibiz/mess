using Mess.Eor.Abstractions.Indexes;
using Mess.Eor.Abstractions.Models;
using Mess.Iot.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Eor.Indexes;

public class EorMeasurementDeviceIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<EorIotDeviceIndex>()
      .When(contentItem => contentItem.Has<EorMeasurementDevicePart>())
      .Map(contentItem =>
      {
        var eorMeasurementDevice =
          contentItem.AsContent<EorMeasurementDeviceItem>();

        return new EorIotDeviceIndex
        {
          ContentItemId = eorMeasurementDevice.ContentItemId,
          DeviceId = eorMeasurementDevice
            .MeasurementDevicePart
            .Value
            .DeviceId
            .Text,
          OwnerId =
            eorMeasurementDevice.EorMeasurementDevicePart.Value.Owner.UserIds.First(),
          Author = eorMeasurementDevice.Inner.Owner
        };
      });
  }
}
