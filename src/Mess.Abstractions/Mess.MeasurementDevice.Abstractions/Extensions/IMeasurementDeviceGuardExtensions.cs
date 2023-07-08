using Mess.MeasurementDevice.Abstractions.Fields;
using Mess.MeasurementDevice.Abstractions.Security;

namespace Mess.MeasurementDevice.Abstractions.Extensions;

public static class IMeasurementGuardExtensions
{
  public static ApiKeyField HashApiKeyField(
    this IMeasurementDeviceGuard measurementDeviceGuard,
    string apiKey
  )
  {
    var apiKeySalt = measurementDeviceGuard.GenerateApiKeySalt();
    var apiKeyHash = measurementDeviceGuard.HashApiKey(apiKey, apiKeySalt);
    return new ApiKeyField { Salt = apiKeySalt, Hash = apiKeyHash };
  }

  public static async Task<ApiKeyField> HashApiKeyFieldAsync(
    this IMeasurementDeviceGuard measurementDeviceGuard,
    string apiKey
  )
  {
    var apiKeySalt = measurementDeviceGuard.GenerateApiKeySalt();
    var apiKeyHash = measurementDeviceGuard.HashApiKey(apiKey, apiKeySalt);
    return new ApiKeyField { Salt = apiKeySalt, Hash = apiKeyHash };
  }
}
