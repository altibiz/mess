using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public record BulkIotPushRequest(
  string DeviceId,
  DateTimeOffset Timestamp,
  ContentItem ContentItem,
  string Request
);

public interface IIotPushHandler
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

  public void HandleBulk(BulkIotPushRequest[] requests);

  public Task HandleBulkAsync(BulkIotPushRequest[] requests);
}
