using Mess.System.Extensions.Objects;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Polling;

public abstract class JsonMeasurementDevicePollHandler<TResponse>
  : IMeasurementDevicePollHandler
{
  public string? Handle(string deviceId, ContentItem contentItem)
  {
    var response = MakeResponse(deviceId, contentItem);

    return response.ToJson();
  }

  public async Task<string?> HandleAsync(
    string deviceId,
    ContentItem contentItem
  )
  {
    var response = await MakeResponseAsync(deviceId, contentItem);

    return response.ToJson();
  }

  public abstract string ContentType { get; }

  protected abstract TResponse MakeResponse(
    string deviceId,
    ContentItem contentItem
  );

  protected abstract Task<TResponse> MakeResponseAsync(
    string deviceId,
    ContentItem contentItem
  );

  protected JsonMeasurementDevicePollHandler(ILogger logger)
  {
    _logger = logger;
  }

  protected ILogger _logger;
}
