using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Pushing;

public interface IMeasurementDevicePushHandler
{
  public string ContentType { get; }

  public bool Handle(string deviceId, ContentItem contentItem, string request);

  public Task<bool> HandleAsync(
    string deviceId,
    ContentItem contentItem,
    string request
  );
}
