using Mess.Cms;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public abstract class IotUpdateHandler<T> : IIotUpdateHandler
  where T : ContentItemBase
{
  public void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    string request
  )
  {
    var item = contentItem.AsContent<T>();
    Handle(deviceId, timestamp, item, request);
  }

  public async Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    string request
  )
  {
    var item = contentItem.AsContent<T>();
    await HandleAsync(deviceId, timestamp, item, request);
  }

  public string ContentType => typeof(T).ContentTypeName();

  protected abstract void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    T contentItem,
    string request
  );

  protected abstract Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    T contentItem,
    string request
  );
}
