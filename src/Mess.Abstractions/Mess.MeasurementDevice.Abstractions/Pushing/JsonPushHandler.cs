using Mess.System.Extensions.Objects;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Pushing;

public abstract class JsonPushHandler<TRequest> : IPushHandler
{
  public bool Handle(string deviceId, ContentItem contentItem, string request)
  {
    var parsedRequest = request.FromJson<TRequest>();
    if (parsedRequest is null)
    {
      _logger.LogWarning(
        $"Request '{request}' could not be deserialized to {typeof(TRequest).Name}"
      );
      return false;
    }

    Handle(deviceId, contentItem, parsedRequest);

    return true;
  }

  public async Task<bool> HandleAsync(
    string deviceId,
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

    await HandleAsync(deviceId, contentItem, parsedRequest);

    return true;
  }

  public abstract string Id { get; }

  protected abstract void Handle(
    string deviceId,
    ContentItem contentItem,
    TRequest request
  );

  protected abstract Task HandleAsync(
    string deviceId,
    ContentItem contentItem,
    TRequest request
  );

  protected JsonPushHandler(ILogger logger)
  {
    _logger = logger;
  }

  protected readonly ILogger _logger;
}
