using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Pushing;

public interface IMeasurementDevicePushHandler
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
