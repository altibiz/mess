using System.Collections.Concurrent;
using Mess.Iot.Abstractions.Caches;
using Mess.Iot.Abstractions.Indexes;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Iot.Caches;

public class IotDeviceContentItemCache : IIotDeviceContentItemCache
{
  private readonly ConcurrentDictionary<
    string,
    Lazy<Task<ContentItem?>>
  > _cache = new();

  private readonly ISession _session;

  public IotDeviceContentItemCache(ISession session)
  {
    _session = session;
  }

  public ContentItem? GetIotDevice(string deviceId)
  {
    return _cache
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
  }

  public async Task<ContentItem?> GetIotDeviceAsync(string deviceId)
  {
    return await _cache
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
  }
}
