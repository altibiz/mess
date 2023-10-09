using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Updating;

public interface IMeasurementDeviceUpdateHandler
{
  public string ContentType { get; }

  public bool Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    string request
  );

  public Task<bool> HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    string request
  );
}
