using System.Text.Json.Serialization;

namespace Mess.EventStore.Abstractions.Events;

public interface IEvent
{
  [JsonIgnore]
  public string Tenant { get; }

  [JsonIgnore]
  public DateTime Timestamp { get; }

  [JsonIgnore]
  public string Payload { get; }
}
