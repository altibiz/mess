using System.Xml.Linq;
using Mess.OrchardCore;
using Mess.System.Extensions.Objects;

namespace Mess.Iot.Abstractions.Services;

public abstract class XmlIotPushHandler<T, R> : IotPushHandler<T>
  where T : ContentItemBase
{
  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem,
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
    T contentItem,
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
    T contentItem,
    R request
  );

  protected abstract Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem,
    R request
  );

  protected abstract R Parse(XDocument xml);
}
