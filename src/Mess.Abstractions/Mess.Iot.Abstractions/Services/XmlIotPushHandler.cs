using System.Xml.Linq;
using Mess.Cms;
using Mess.Prelude.Extensions.Objects;

namespace Mess.Iot.Abstractions.Services;

public record BulkIotXmlPushRequest<TItem, TRequest>(
  string DeviceId,
  DateTimeOffset Timestamp,
  TItem ContentItem,
  TRequest Request
) where TItem : ContentItemBase;

public abstract class XmlIotPushHandler<TItem, TRequest>
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

  protected abstract void HandleBulk(BulkIotXmlPushRequest<TItem, TRequest>[] requests);

  protected abstract Task HandleBulkAsync(BulkIotXmlPushRequest<TItem, TRequest>[] requests);

  protected override void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem,
    string request
  )
  {
    var xml = request.FromXml();
    var parsed = Parse(xml);
    Handle(deviceId, timestamp, contentItem, parsed);
  }

  protected override async Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem,
    string request
  )
  {
    var xml = request.FromXml();
    var parsedRequest = Parse(xml);
    await HandleAsync(deviceId, timestamp, contentItem, parsedRequest);
  }

  protected override void HandleBulk(BulkIotItemPushRequest<TItem>[] requests)
  {
    var xml = requests
      .Select(request =>
      {
        var parsed = Parse(request.Request.FromXml());
        return new BulkIotXmlPushRequest<TItem, TRequest>(
          request.DeviceId,
          request.Timestamp,
          request.ContentItem,
          parsed
        );
      }).ToArray();
    HandleBulk(xml);
  }

  protected override async Task HandleBulkAsync(BulkIotItemPushRequest<TItem>[] requests)
  {
    var xml = requests
      .Select(request =>
      {
        var parsed = Parse(request.Request.FromXml());
        return new BulkIotXmlPushRequest<TItem, TRequest>(
          request.DeviceId,
          request.Timestamp,
          request.ContentItem,
          parsed
        );
      }).ToArray();
    await HandleBulkAsync(xml);
  }

  protected abstract TRequest Parse(XDocument xml);
}
