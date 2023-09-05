using Mess.ContentFields.Abstractions.Fields;
using Mess.MeasurementDevice.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Security;

public class RaspberryPiAuthorizationHandler
  : ApiKeyMeasurementDeviceAuthorizationHandler
{
  public const string AuthorizationContentType = "RaspberryPiMeasurementDevice";

  public override string ContentType => AuthorizationContentType;

  public override ApiKeyField? GetApiKey(ContentItem measurementDevice)
  {
    var raspberryPiMeasurementDevice =
      measurementDevice.As<RaspberryPiMeasurementDevicePart>();
    return raspberryPiMeasurementDevice?.ApiKey;
  }
}
