using Mess.Fields.Abstractions.Fields;
using Mess.Iot.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;

namespace Mess.Ozds.Iot;

public class PidgeonAuthorizationHandler
  : ApiKeyIotAuthorizationHandler<PidgeonIotDeviceItem>
{
  protected override ApiKeyField GetApiKey(PidgeonIotDeviceItem contentItem)
  {
    return contentItem.PidgeonIotDevicePart.Value.ApiKey;
  }
}
