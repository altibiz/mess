using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public interface IMeasurementDeviceContentItemCache
{
  public Task<ContentItem?> GetAsync(string deviceId);

  public ContentItem? Get(string deviceId);
}
