using System.Collections.Concurrent;
using Mess.MeasurementDevice.Abstractions.Indexes;
using Mess.MeasurementDevice.Abstractions.Services;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.MeasurementDevice.Services;

public class MeasurementDeviceContentItemCache
  : IMeasurementDeviceContentItemCache
{
  public ContentItem? Get(string deviceId)
  {
    return _cache
      .GetOrAdd(
        deviceId,
        _ =>
          new Lazy<Task<ContentItem?>>(
            () =>
              _session
                .Query<ContentItem?, MeasurementDeviceIndex>()
                .Where(device => device.DeviceId == deviceId)
                .FirstOrDefaultAsync()
          )
      )
      .Value.Result;
  }

  public async Task<ContentItem?> GetAsync(string deviceId)
  {
    return await _cache
      .GetOrAdd(
        deviceId,
        _ =>
          new Lazy<Task<ContentItem?>>(
            () =>
              _session
                .Query<ContentItem?, MeasurementDeviceIndex>()
                .Where(device => device.DeviceId == deviceId)
                .FirstOrDefaultAsync()
          )
      )
      .Value;
  }

  public MeasurementDeviceContentItemCache(ISession session)
  {
    _session = session;
  }

  private readonly ISession _session;

  private readonly ConcurrentDictionary<
    string,
    Lazy<Task<ContentItem?>>
  > _cache = new ConcurrentDictionary<string, Lazy<Task<ContentItem?>>>();
}
