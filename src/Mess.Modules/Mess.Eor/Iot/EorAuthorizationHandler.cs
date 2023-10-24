using Mess.Eor.Abstractions.Models;
using Mess.Fields.Abstractions.Fields;
using Mess.Iot.Abstractions.Services;

namespace Mess.Eor.Iot;

public class EorAuthorizationHandler
  : ApiKeyIotAuthorizationHandler<EorIotDeviceItem>
{
  protected override ApiKeyField GetApiKey(EorIotDeviceItem contentItem)
  {
    return contentItem.EorIotDevicePart.Value.ApiKey;
  }
}
