using Mess.Cms;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public abstract class IotPushHandler<T> : IIotPushHandler
  where T : ContentItemBase
{
  protected abstract void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem,
    string request
  );

  protected abstract Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem,
    string request
  );

  public void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    string request
  )
  {
    var item = contentItem.AsContent<T>();
    Handle(deviceId, tenant, timestamp, item, request);
  }

  public async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    string request
  )
  {
    var item = contentItem.AsContent<T>();
    await HandleAsync(deviceId, tenant, timestamp, item, request);
  }

  public string ContentType => typeof(T).ContentTypeName();
}
