using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Updating;

public interface IUpdateHandler
{
  public string Id { get; }

  public bool Handle(string deviceId, ContentItem contentItem, string request);

  public Task<bool> HandleAsync(
    string deviceId,
    ContentItem contentItem,
    string request
  );
}
