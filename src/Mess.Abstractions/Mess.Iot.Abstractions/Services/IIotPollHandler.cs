using OrchardCore.ContentManagement;

namespace Mess.Iot.Abstractions.Services;

public interface IIotPollHandler
{
  public string ContentType { get; }

  public string Handle(
    string deviceId,
    DateTimeOffset timestamp,
    ContentItem contentItem
  );

  public Task<string> HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    ContentItem contentItem
  );
}
