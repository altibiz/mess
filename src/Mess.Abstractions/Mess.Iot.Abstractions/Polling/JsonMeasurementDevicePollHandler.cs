using Mess.System.Extensions.Objects;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Polling;

public abstract class JsonMeasurementDevicePollHandler<TResponse>
  : IMeasurementDevicePollHandler
{
  public string? Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem
  )
  {
    var response = MakeResponse(deviceId, tenant, timestamp, contentItem);

    return response.ToJson();
  }

  public async Task<string?> HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem
  )
  {
    var response = await MakeResponseAsync(
      deviceId,
      tenant,
      timestamp,
      contentItem
    );

    return response.ToJson();
  }

  public abstract string ContentType { get; }

  protected abstract TResponse MakeResponse(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem
  );

  protected abstract Task<TResponse> MakeResponseAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem
  );

  protected JsonMeasurementDevicePollHandler(ILogger logger)
  {
    _logger = logger;
  }

  protected ILogger _logger;
}
