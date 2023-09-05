using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Services;

public interface IMeasurementDeviceContentItemCache
{
  public Task<ContentItem?> GetAsync(string deviceId);

  public ContentItem? Get(string deviceId);
}
