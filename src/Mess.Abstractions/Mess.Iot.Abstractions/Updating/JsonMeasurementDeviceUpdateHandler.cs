using Mess.System.Extensions.Objects;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Updating;

public abstract class JsonMeasurementDeviceUpdateHandler<TStatus>
  : IMeasurementDeviceUpdateHandler
{
  public bool Handle(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    string request
  )
  {
    var measurementObject = request.FromJson<TStatus>();
    if (measurementObject is null)
    {
      _logger.LogWarning(
        $"Request '{request}' could not be deserialized to {typeof(TStatus).Name}"
      );
      return false;
    }

    Handle(deviceId, tenant, timestamp, contentItem, measurementObject);

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
    var measurementObject = request.FromJson<TStatus>();
    if (measurementObject is null)
    {
      _logger.LogWarning(
        $"Request '{request}' could not be deserialized to {typeof(TStatus).Name}"
      );
      return false;
    }

    await HandleAsync(
      deviceId,
      tenant,
      timestamp,
      contentItem,
      measurementObject
    );

    return true;
  }

  public abstract string ContentType { get; }

  protected abstract void Handle(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    TStatus request
  );

  protected abstract Task HandleAsync(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    TStatus request
  );

  protected JsonMeasurementDeviceUpdateHandler(ILogger logger)
  {
    _logger = logger;
  }

  protected readonly ILogger _logger;
}