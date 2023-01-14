using Microsoft.AspNetCore.Mvc;
using Mess.EventStore.Timeseries;
using Mess.EventStore.Abstractions.Client;
using OrchardCore.Admin;
using Mess.EventStore.ViewModels;

namespace Mess.EventStore.Controllers;

[Admin]
public class AdminController : Controller
{
  public async Task<IActionResult> Index([FromServices] IEventStoreClient store)
  {
    // TODO: extract
    var lastEvent = await store.LastEventAsync<EgaugeMeasured>();

    if (lastEvent is null)
    {
      return View(new EventStoreViewModel(DateTime.UtcNow));
    }

    return View(new EventStoreViewModel(lastEvent.timestamp));
  }
}
