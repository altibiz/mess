using Mess.ContentFields.Abstractions.Fields;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.MeasurementDevice.Abstractions.Security;
using OrchardCore.ContentManagement;

namespace Mess.Eor.MeasurementDevice.Security;

public class EorAuthorizationHandler
  : ApiKeyMeasurementDeviceAuthorizationHandler
{
  public const string AuthorizationContentType = "EorMeasurementDevice";

  public override string ContentType => AuthorizationContentType;

  public override ApiKeyField? GetApiKey(ContentItem measurementDevice)
  {
    var eorMeasurementDevice = measurementDevice.As<EorMeasurementDevicePart>();
    return eorMeasurementDevice?.ApiKey;
  }
}
