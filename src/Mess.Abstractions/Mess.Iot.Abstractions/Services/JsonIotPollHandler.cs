using Mess.Cms;
using Mess.Prelude.Extensions.Objects;

namespace Mess.Iot.Abstractions.Services;

public abstract class JsonIotPollHandler<TItem, TResponse>
  : IotPollHandler<TItem>
  where TItem : ContentItemBase
{
  protected abstract TResponse MakeResponse(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem
  );

  protected abstract Task<TResponse> MakeResponseAsync(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem
  );

  protected override string Handle(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem
  )
  {
    var response = MakeResponse(deviceId, timestamp, contentItem);
    return response.ToJson();
  }

  protected override async Task<string> HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    TItem contentItem
  )
  {
    var response = MakeResponseAsync(deviceId, timestamp, contentItem);
    return response.ToJson();
  }
}
