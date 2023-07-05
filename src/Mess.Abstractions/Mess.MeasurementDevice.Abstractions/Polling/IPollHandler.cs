using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Polling;

public interface IPollHandler
{
  public string Id { get; }

  public string? Handle(string deviceId, ContentItem contentItem);

  public Task<string?> HandleAsync(string deviceId, ContentItem contentItem);
}
