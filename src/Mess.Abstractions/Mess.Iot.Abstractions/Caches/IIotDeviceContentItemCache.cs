using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Caches;

public interface IIotDeviceContentItemCache
{
  public Task<ContentItem?> GetAsync(string deviceId);

  public ContentItem? Get(string deviceId);
}
