using System.Collections.Concurrent;
using Mess.Iot.Abstractions.Caches;
using Mess.Iot.Abstractions.Indexes;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Iot.Caches;

public class IotDeviceContentItemCache : IIotDeviceContentItemCache
{
  private readonly ConcurrentDictionary<
    string,
    Lazy<Task<ContentItem?>>
  > _cache = new();

  private readonly IServiceProvider _services;

  public IotDeviceContentItemCache(IServiceProvider services)
  {
    _services = services;
  }

  public ContentItem? GetIotDevice(string deviceId)
  {
    return _cache
      .GetOrAdd(
        deviceId,
        _ =>
          new Lazy<Task<ContentItem?>>(
            async () =>
            {
              await using var scope = _services.CreateAsyncScope();
              return await scope.ServiceProvider
                .GetRequiredService<ISession>()
                .Query<ContentItem?, IotDeviceIndex>()
                .Where(device => device.DeviceId == deviceId)
                .FirstOrDefaultAsync();
            }
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
            async () =>
            {
              await using var scope = _services.CreateAsyncScope();
              return await scope.ServiceProvider
                .GetRequiredService<ISession>()
                .Query<ContentItem?, IotDeviceIndex>()
                .Where(device => device.DeviceId == deviceId)
                .FirstOrDefaultAsync();
            }
          )
      )
      .Value;
  }
}
