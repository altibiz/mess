using System.Xml.Linq;
using Mess.System.Extensions.Objects;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Pushing;

public abstract class XmlMeasurementDevicePushHandler<TRequest>
  : IMeasurementDevicePushHandler
{
  public bool Handle(string deviceId, ContentItem contentItem, string request)
  {
    var xml = request.FromXml();
    if (xml is null)
    {
      _logger.LogWarning(
        $"Request '{request}' could not be deserialized to xml"
      );
      return false;
    }

    var parsedRequest = Parse(xml);
    if (parsedRequest is null)
    {
      _logger.LogWarning(
        $"Request '{request}' could not be parsed to {typeof(TRequest).Name}"
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
    var xml = request.FromXml();
    if (xml is null)
    {
      _logger.LogWarning(
        $"Measurement '{request}' could not be deserialized to xml"
      );
      return false;
    }

    var parsedRequest = Parse(xml);
    if (parsedRequest is null)
    {
      _logger.LogWarning(
        $"Request '{request}' could not be parsed to {typeof(TRequest).Name}"
      );
      return false;
    }

    await HandleAsync(deviceId, contentItem, parsedRequest);

    return true;
  }

  public abstract string ContentType { get; }

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

  protected abstract TRequest? Parse(XDocument xml);

  protected XmlMeasurementDevicePushHandler(ILogger logger)
  {
    _logger = logger;
  }

  protected readonly ILogger _logger;
}
