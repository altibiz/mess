using Newtonsoft.Json.Linq;
using OrchardCore.Recipes.Models;
using OrchardCore.Recipes.Services;
using Mess.EventStore.Abstractions.Client;
using Mess.System.Extensions.Microsoft;
using Mess.EventStore.Abstractions.Events;

namespace Mess.EventStore.Recipes;

public class EventImportStep : IRecipeStepHandler
{
  // Example:
  // {
  //   "name": "EventImport",
  //   "Streams": [
  //     {
  //       "AggregateType": "Mess.EventStore.Events.EventStream",
  //       "Events": [
  //         {
  //           "EventType": "Mess.EventsStore.Events.Event",
  //           "Data": {
  //             "Property": "Value"
  //           }
  //         }
  //       ]
  //     }
  //   ]
  // }
  public async Task ExecuteAsync(RecipeExecutionContext context)
  {
    if (
      !String.Equals(
        context.Name,
        "EventImport",
        StringComparison.OrdinalIgnoreCase
      )
    )
    {
      return;
    }

    var streams = context.Step.Property("Streams")?.Value as JArray;
    if (streams is null)
    {
      return;
    }

    foreach (JObject stream in streams)
    {
      var aggregateTypeName = (string?)(
        stream.Property("AggregateType")?.Value as JValue
      );
      if (aggregateTypeName is null)
      {
        continue;
      }

      var aggregateType = AppDomain.CurrentDomain.FindTypeByName(
        aggregateTypeName
      );
      if (aggregateType is null)
      {
        continue;
      }

      var events = (stream.Property("Events")?.Value as JArray)
        ?.Select(
          (@event) =>
          {
            var eventObject = @event as JObject;
            if (eventObject is null)
            {
              return null;
            }

            var eventTypeName = (string?)(
              eventObject.Property("EventType")?.Value as JValue
            );
            if (eventTypeName is null)
            {
              return null;
            }

            var eventType = AppDomain.CurrentDomain.FindTypeByName(
              eventTypeName
            );
            if (eventType is null)
            {
              return null;
            }

            var eventData = eventObject.Property("Data")?.Value as JObject;
            if (eventData is null)
            {
              return null;
            }

            return eventData.ToObject(eventType);
          }
        )
        .Cast<IEvent>()
        .Where(@event => @event is not null);
      if (events is null)
      {
        continue;
      }

      await _client.RecordEventsAsync(aggregateType, events.ToArray());
    }
  }

  public EventImportStep(IEventStoreClient client)
  {
    _client = client;
  }

  private readonly IEventStoreClient _client;
}
