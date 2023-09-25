using Mess.ContentFields.Abstractions.Fields;
using Mess.MeasurementDevice.Abstractions.Security;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Security;

public class PidgeonAuthorizationHandler
  : ApiKeyMeasurementDeviceAuthorizationHandler
{
  public const string AuthorizationContentType = "PidgeonMeasurementDevice";

  public override string ContentType => AuthorizationContentType;

  public override ApiKeyField? GetApiKey(ContentItem measurementDevice)
  {
    var pidgeonMeasurementDevice =
      measurementDevice.As<PidgeonMeasurementDevicePart>();
    return pidgeonMeasurementDevice?.ApiKey;
  }
}
