using Mess.MeasurementDevice.Abstractions.Models;

namespace Mess.MeasurementDevice.Abstractions.Security;

public interface IMeasurementDeviceGuard
{
  Task<byte[]> GenerateApiKeySaltAsync();

  byte[] GenerateApiKeySalt();

  Task<string> HashApiKeyAsync(string apiKey, byte[] salt);

  string HashApiKey(string apiKey, byte[] salt);

  bool Authorize(MeasurementDevicePart measurementDevice, string apiKey);

  Task<bool> AuthorizeAsync(
    MeasurementDevicePart measurementDevice,
    string apiKey
  );
}
