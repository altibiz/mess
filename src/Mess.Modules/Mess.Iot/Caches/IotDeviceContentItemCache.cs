using System.Collections.Concurrent;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Iot.Abstractions.Caches;
using Mess.Iot.Abstractions.Indexes;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Iot.Caches;

public class IotDeviceContentItemCache : IIotDeviceContentItemCache
{
  private readonly IServiceProvider _services;

  public IotDeviceContentItemCache(IServiceProvider services)
  {
    _services = services;
  }

  public ContentItem? GetIotDevice(string deviceId)
  {
    using var scope = _services.CreateScope();
    return scope.ServiceProvider
      .GetRequiredService<ISession>()
      .Query<ContentItem?, IotDeviceIndex>()
      .Where(device => device.DeviceId == deviceId)
      .LatestPublished()
      .FirstOrDefaultAsync().Result;
  }

  public async Task<ContentItem?> GetIotDeviceAsync(string deviceId)
  {
    await using var scope = _services.CreateAsyncScope();
    return await scope.ServiceProvider
      .GetRequiredService<ISession>()
      .Query<ContentItem?, IotDeviceIndex>()
      .Where(device => device.DeviceId == deviceId)
      .LatestPublished()
      .FirstOrDefaultAsync();
  }
}
