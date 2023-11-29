using Newtonsoft.Json.Linq;
using OrchardCore.Recipes.Models;
using OrchardCore.Recipes.Services;
using Mess.Event.Abstractions.Client;
using Mess.System.Extensions.Microsoft;
using Mess.Event.Abstractions.Services;

namespace Mess.Event.Recipes;

public class EventImportStep : IRecipeStepHandler
{
  // Example:
  // {
  //   "name": "EventImport",
  //   "Streams": [
  //     {
  //       "AggregateType": "Mess.Event.Events.EventStream",
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
      !string.Equals(
        context.Name,
        "EventImport",
        StringComparison.OrdinalIgnoreCase
      )
    )
    {
      return;
    }

    var streams = context.Step.Property("Streams")?.Value;
    if (streams is not JArray)
    {
      return;
    }

    foreach (JObject stream in streams.Cast<JObject>())
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

            if (@event is not JObject eventObject)
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

            var eventData = eventObject.Property("Data")?.Value;

            return eventData is not JObject ? null : eventData.ToObject(eventType);
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
