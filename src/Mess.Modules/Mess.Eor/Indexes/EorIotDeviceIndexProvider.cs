using Mess.Eor.Abstractions.Indexes;
using Mess.Eor.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Mess.Eor.Indexes;

public class EorIotDeviceIndexProvider : IndexProvider<ContentItem>
{
  public override void Describe(DescribeContext<ContentItem> context)
  {
    context
      .For<EorIotDeviceIndex>()
      .When(contentItem => contentItem.Has<EorIotDevicePart>())
      .Map(contentItem =>
      {
        var eorIotDevice = contentItem.AsContent<EorIotDeviceItem>();

        return new EorIotDeviceIndex
        {
          ContentItemId = eorIotDevice.ContentItemId,
          DeviceId = eorIotDevice.IotDevicePart.Value.DeviceId.Text,
          OwnerId = eorIotDevice.EorIotDevicePart.Value.Owner.UserIds.First(),
          Author = eorIotDevice.Inner.Owner
        };
      });
  }
}
