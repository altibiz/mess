using Mess.Blazor.Abstractions.Controllers;
using Mess.Blazor.Abstractions.Extensions;
using Mess.Blazor.Components;
using Microsoft.AspNetCore.Mvc;

// TODO: app access permission

namespace Mess.Blazor.Controllers;

public class AppController : Controller
{
  public async Task<IActionResult> Index()
  {
    return await this.Component(typeof(App), null);
  }
}
