using System.Text.Json.Serialization;

namespace Mess.Event.Abstractions.Services;

public interface IEvent
{
  [JsonIgnore] public string Tenant { get; }

  [JsonIgnore] public DateTimeOffset Timestamp { get; }

  [JsonIgnore] public string Payload { get; }
}
