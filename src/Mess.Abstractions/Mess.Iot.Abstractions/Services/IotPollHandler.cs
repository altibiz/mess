using Mess.OrchardCore;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public abstract class IotPollHandler<T> : IIotPollHandler
  where T : ContentItemBase
{
  protected abstract string Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem
  );

  protected abstract Task<string> HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem
  );

  public string Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem
  )
  {
    var item = contentItem.AsContent<T>();
    return Handle(deviceId, tenant, timestamp, item);
  }

  public async Task<string> HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem
  )
  {
    var item = contentItem.AsContent<T>();
    return await HandleAsync(deviceId, tenant, timestamp, item);
  }

  public string ContentType => typeof(T).ContentTypeName();
}