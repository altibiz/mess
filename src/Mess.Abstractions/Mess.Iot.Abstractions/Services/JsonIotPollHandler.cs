using Mess.OrchardCore;
using Mess.System.Extensions.Objects;

namespace Mess.Iot.Abstractions.Services;

public abstract class JsonIotPollHandler<T, R> : IotPollHandler<T>
  where T : ContentItemBase
{
  protected abstract R MakeResponse(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem
  );

  protected abstract Task<R> MakeResponseAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem
  );

  protected override string Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem
  )
  {
    var response = MakeResponse(deviceId, tenant, timestamp, contentItem);
    return response.ToJson();
  }

  protected override async Task<string> HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    T contentItem
  )
  {
    var response = MakeResponseAsync(deviceId, tenant, timestamp, contentItem);
    return response.ToJson();
  }
}
