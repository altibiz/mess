using System.Collections.Concurrent;
using Mess.Iot.Abstractions.Indexes;
using Mess.Iot.Abstractions.Caches;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Iot.Caches;

public class IotDeviceContentItemCache : IIotDeviceContentItemCache
{
  public ContentItem? GetIotDevice(string deviceId) =>
    _cache
      .GetOrAdd(
        deviceId,
        _ =>
          new Lazy<Task<ContentItem?>>(
            () =>
              _session
                .Query<ContentItem?, IotDeviceIndex>()
                .Where(device => device.DeviceId == deviceId)
                .FirstOrDefaultAsync()
          )
      )
      .Value.Result;

  public async Task<ContentItem?> GetIotDeviceAsync(string deviceId) =>
    await _cache
      .GetOrAdd(
        deviceId,
        _ =>
          new Lazy<Task<ContentItem?>>(
            () =>
              _session
                .Query<ContentItem?, IotDeviceIndex>()
                .Where(device => device.DeviceId == deviceId)
                .FirstOrDefaultAsync()
          )
      )
      .Value;

  public IotDeviceContentItemCache(ISession session)
  {
    _session = session;
  }

  private readonly ISession _session;

  private readonly ConcurrentDictionary<
    string,
    Lazy<Task<ContentItem?>>
  > _cache = new();
}
