using System.Xml.Linq;
using Mess.Cms;
using Mess.Prelude.Extensions.Objects;

namespace Mess.Iot.Abstractions.Services;

public abstract class XmlIotPushHandler<TItem, TResponse>
  : IotPushHandler<TItem>
  where TItem : ContentItemBase
{
  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    TItem contentItem,
    string request
  )
  {
    var xml = request.FromXml();
    var parsed = Parse(xml);
    Handle(deviceId, tenant, timestamp, contentItem, parsed);
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    TItem contentItem,
    string request
  )
  {
    var xml = request.FromXml();
    var parsedRequest = Parse(xml);
    await HandleAsync(deviceId, tenant, timestamp, contentItem, parsedRequest);
  }

  protected abstract void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    TItem contentItem,
    TResponse request
  );

  protected abstract Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    TItem contentItem,
    TResponse request
  );

  protected abstract TResponse Parse(XDocument xml);
}
