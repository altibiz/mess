using Mess.Cms;
using Mess.Prelude.Extensions.Objects;

namespace Mess.Iot.Abstractions.Services;

public record BulkIotJsonPushRequest<TItem, TRequest>(
  string DeviceId,
  DateTimeOffset Timestamp,
  TItem ContentItem,
  TRequest Request
) where TItem : ContentItemBase;

public abstract class JsonIotPushHandler<TItem, TRequest>
  : IotPushHandler<TItem>
  where TItem : ContentItemBase
{
  protected abstract void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem,
    TRequest request
  );

  protected abstract Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem,
    TRequest request
  );

  protected abstract void HandleBulk(BulkIotJsonPushRequest<TItem, TRequest>[] requests);

  protected abstract Task HandleBulkAsync(BulkIotJsonPushRequest<TItem, TRequest>[] requests);

  protected override void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem,
    string request
  )
  {
    var json = request.FromJson<TRequest>();
    Handle(deviceId, timestamp, contentItem, json);
  }

  protected override async Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem,
    string request
  )
  {
    var json = request.FromJson<TRequest>();
    await HandleAsync(deviceId, timestamp, contentItem, json);
  }

  protected override void HandleBulk(BulkIotItemPushRequest<TItem>[] requests)
  {
    var json = requests
      .Select(request =>
      {
        var parsed = request.Request.FromJson<TRequest>();
        return new BulkIotJsonPushRequest<TItem, TRequest>(
          request.DeviceId,
          request.Timestamp,
          request.ContentItem,
          parsed
        );
      }).ToArray();
    HandleBulk(json);
  }

  protected override async Task HandleBulkAsync(BulkIotItemPushRequest<TItem>[] requests)
  {
    var json = requests
      .Select(request =>
      {
        var parsed = request.Request.FromJson<TRequest>();
        return new BulkIotJsonPushRequest<TItem, TRequest>(
          request.DeviceId,
          request.Timestamp,
          request.ContentItem,
          parsed
        );
      }).ToArray();
    await HandleBulkAsync(json);
  }
}
