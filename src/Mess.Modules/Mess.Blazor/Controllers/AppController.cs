using Mess.Blazor.Abstractions.Controllers;
using Mess.Blazor.Abstractions.Extensions;
using Mess.Blazor.Components;
using Microsoft.AspNetCore.Mvc;

// TODO: app access permission, app enable configuration

namespace Mess.Blazor.Controllers;

public class AppController : Controller
{
  public async Task<IActionResult> Index()
  {
    return View("/Areas/Mess.Blazor/Views/Shared/App.cshtml");
  }
}
