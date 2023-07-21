using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Polling;

public interface IMeasurementDevicePollHandler
{
  public string ContentType { get; }

  public string? Handle(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem
  );

  public Task<string?> HandleAsync(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem
  );
}
