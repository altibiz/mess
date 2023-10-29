using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Caches;

public interface IIotDeviceContentItemCache
{
  public Task<ContentItem?> GetIotDeviceAsync(string deviceId);

  public ContentItem? GetIotDevice(string deviceId);
}
