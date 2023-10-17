using Mess.OrchardCore;
using Mess.System.Extensions.Objects;

namespace Mess.Iot.Abstractions.Services;

public abstract class JsonIotPushHandler<T, R> : IotPushHandler<T>
  where T : ContentItemBase
{
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

  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem,
    string request
  )
  {
    var json = request.FromJson<R>();
    Handle(deviceId, tenant, timestamp, contentItem, json);
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem,
    string request
  )
  {
    var json = request.FromJson<R>();
    await HandleAsync(deviceId, tenant, timestamp, contentItem, json);
  }
}
