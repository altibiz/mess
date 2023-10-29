using Mess.OrchardCore;
using Mess.System.Extensions.Objects;

namespace Mess.Iot.Abstractions.Services;

public abstract class JsonIotPollHandler<TItem, TResponse>
  : IotPollHandler<TItem>
  where TItem : ContentItemBase
{
  protected abstract TResponse MakeResponse(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    TItem contentItem
  );

  protected abstract Task<TResponse> MakeResponseAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    TItem contentItem
  );

  protected override string Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    TItem contentItem
  )
  {
    var response = MakeResponse(deviceId, tenant, timestamp, contentItem);
    return response.ToJson();
  }

  protected override async Task<string> HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    TItem contentItem
  )
  {
    var response = MakeResponseAsync(deviceId, tenant, timestamp, contentItem);
    return response.ToJson();
  }
}
