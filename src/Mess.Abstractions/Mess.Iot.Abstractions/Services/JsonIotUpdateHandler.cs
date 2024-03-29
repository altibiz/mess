using Mess.Cms;
using Mess.Prelude.Extensions.Objects;

namespace Mess.Iot.Abstractions.Services;

public abstract class JsonIotUpdateHandler<TItem, TResponse>
  : IotUpdateHandler<TItem>
  where TItem : ContentItemBase
{
  protected abstract void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem,
    TResponse request
  );

  protected abstract Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem,
    TResponse request
  );

  protected override void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem,
    string request
  )
  {
    var json = request.FromJson<TResponse>();
    Handle(deviceId, timestamp, contentItem, json);
  }

  protected override async Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem,
    string request
  )
  {
    var json = request.FromJson<TResponse>();
    await HandleAsync(deviceId, timestamp, contentItem, json);
  }
}
