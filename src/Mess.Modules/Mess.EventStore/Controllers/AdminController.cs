using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using Mess.EventStore.Abstractions.Client;
using Mess.EventStore.ViewModels;

namespace Mess.EventStore.Controllers;

[Admin]
public class AdminController : Controller
{
  public IActionResult Index([FromServices] IEventStoreClient store)
  {
    // TODO: basic stats
    return View(new EventStoreViewModel(DateTime.UtcNow));
  }
}
