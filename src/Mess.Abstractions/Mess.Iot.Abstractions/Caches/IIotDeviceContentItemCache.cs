using Mess.Cms;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Caches;

public interface IIotDeviceContentItemCache
{
  public Task<ContentItem?> GetIotDeviceAsync(string deviceId);

  public ContentItem? GetIotDevice(string deviceId);
}

public static class IIotDeviceContentItemCacheExtensions
{
  public static async Task<T?> GetIotDeviceContentAsync<T>(this IIotDeviceContentItemCache cache, string deviceId) where T : ContentItemBase
  {
    return (await cache.GetIotDeviceAsync(deviceId))?.AsContent<T>();
  }

  public static T? GetIotDeviceContent<T>(this IIotDeviceContentItemCache cache, string deviceId) where T : ContentItemBase
  {
    return cache.GetIotDevice(deviceId)?.AsContent<T>();
  }

}
