using Mess.System.Extensions.Objects;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Pushing;

public abstract class JsonMeasurementDevicePushHandler<TRequest>
  : IMeasurementDevicePushHandler
{
  public bool Handle(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    string request
  )
  {
    var parsedRequest = request.FromJson<TRequest>();
    if (parsedRequest is null)
    {
      _logger.LogWarning(
        $"Request '{request}' could not be deserialized to {typeof(TRequest).Name}"
      );
      return false;
    }

    Handle(deviceId, tenant, timestamp, contentItem, parsedRequest);

    return true;
  }

  public async Task<bool> HandleAsync(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    string request
  )
  {
    var parsedRequest = request.FromJson<TRequest>();
    if (parsedRequest is null)
    {
      _logger.LogWarning(
        $"Request '{request}' could not be deserialized to {typeof(TRequest).Name}"
      );
      return false;
    }

    await HandleAsync(deviceId, tenant, timestamp, contentItem, parsedRequest);

    return true;
  }

  public abstract string ContentType { get; }

  protected abstract void Handle(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    TRequest request
  );

  protected abstract Task HandleAsync(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    TRequest request
  );

  protected JsonMeasurementDevicePushHandler(ILogger logger)
  {
    _logger = logger;
  }

  protected readonly ILogger _logger;
}
