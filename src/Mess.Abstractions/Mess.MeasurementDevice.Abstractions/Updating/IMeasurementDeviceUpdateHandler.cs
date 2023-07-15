using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Updating;

public interface IMeasurementDeviceUpdateHandler
{
  public string ContentType { get; }

  public bool Handle(string deviceId, ContentItem contentItem, string request);

  public Task<bool> HandleAsync(
    string deviceId,
    ContentItem contentItem,
    string request
  );
}
