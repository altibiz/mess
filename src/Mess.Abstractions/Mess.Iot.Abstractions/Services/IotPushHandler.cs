using Mess.Cms;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public record BulkIotItemPushRequest<T>(
  string DeviceId,
  DateTimeOffset Timestamp,
  T ContentItem,
  string Request
) where T : ContentItemBase;

public abstract class IotPushHandler<T> : IIotPushHandler
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

  public void HandleBulk(BulkIotPushRequest[] requests)
  {
    HandleBulk(
      requests.Select(request =>
      {
        var item = request.ContentItem.AsContent<T>();
        return new BulkIotItemPushRequest<T>(
          request.DeviceId,
          request.Timestamp,
          item,
          request.Request
        );
      }).ToArray()
    );
  }

  public async Task HandleBulkAsync(BulkIotPushRequest[] requests)
  {
    await HandleBulkAsync(
      requests.Select(request =>
      {
        var item = request.ContentItem.AsContent<T>();
        return new BulkIotItemPushRequest<T>(
          request.DeviceId,
          request.Timestamp,
          item,
          request.Request
        );
      }).ToArray()
    );
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

  protected abstract void HandleBulk(BulkIotItemPushRequest<T>[] requests);

  protected abstract Task HandleBulkAsync(BulkIotItemPushRequest<T>[] requests);
}
