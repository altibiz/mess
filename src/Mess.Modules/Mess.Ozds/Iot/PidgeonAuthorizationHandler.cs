using Mess.Fields.Abstractions.Fields;
using Mess.Iot.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;

namespace Mess.Ozds.Security;

public class PidgeonAuthorizationHandler
  : ApiKeyIotAuthorizationHandler<PidgeonIotDeviceItem>
{
  public override ApiKeyField GetApiKey(PidgeonIotDeviceItem contentItem)
  {
    return contentItem.PidgeonIotDevicePart.Value.ApiKey;
  }
}
