using Mess.EventStore.Abstractions.Client;
using Newtonsoft.Json.Linq;
using OrchardCore.Deployment;

namespace Mess.EventStore.Deployment;

public class AllEventsDeploymentSource : IDeploymentSource
{
  public async Task ProcessDeploymentStepAsync(
    DeploymentStep step,
    DeploymentPlanResult result
  )
  {
    var allTemplatesStep = step as AllEventsDeploymentStep;

    if (allTemplatesStep is null)
    {
      return;
    }

    var streams = await _client.ExportAsync();

    var streamObjects = new JArray();
    foreach (var (aggregateType, events) in streams)
    {
      var eventObjects = new JArray();
      foreach (var @event in events)
      {
        eventObjects.Add(
          new JObject(
            new JProperty("EventType", @event.GetType().FullName),
            new JProperty("Data", JObject.FromObject(@event))
          )
        );
      }

      streamObjects.Add(
        new JObject(
          new JProperty("AggregateType", aggregateType.FullName),
          new JProperty("Events", eventObjects)
        )
      );
    }

    result.Steps.Add(
      new JObject(
        new JProperty("name", "EventImport"),
        new JProperty("Streams", streamObjects)
      )
    );
  }

  public AllEventsDeploymentSource(IEventStoreClient client)
  {
    _client = client;
  }

  private readonly IEventStoreClient _client;
}
