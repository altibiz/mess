using Mess.Eor.Abstractions.Models;
using Mess.Fields.Abstractions.Fields;
using Mess.Iot.Abstractions.Services;

namespace Mess.Eor.Iot;

public class EorAuthorizationHandler
  : ApiKeyIotAuthorizationHandler<EorMeasurementDeviceItem>
{
  public override ApiKeyField GetApiKey(EorMeasurementDeviceItem contentItem)
  {
    return contentItem.EorMeasurementDevicePart.Value.ApiKey;
  }
}
