using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public interface IIotPushHandler
{
  public string ContentType { get; }

  public void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    string request
  );

  public Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    string request
  );
}
