using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public interface IIotUpdateHandler
{
  public string ContentType { get; }

  public void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    string request
  );

  public Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    string request
  );
}
