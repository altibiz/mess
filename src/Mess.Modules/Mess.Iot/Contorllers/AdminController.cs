using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using YesSql;
using Mess.Iot.Abstractions;
using Mess.Iot.Abstractions.Indexes;
using Mess.Iot.ViewModels;
using OrchardCore.ContentManagement;

namespace Mess.Iot.Controllers;

[Admin]
public class AdminController : Controller
{
  public async Task<IActionResult> ListIotDevices()
  {
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        Permissions.ListIotDevices
      )
    )
    {
      return Unauthorized();
    }

    var devices = await _session
      .Query<ContentItem, IotDeviceIndex>()
      .ListAsync();

    return View(new IotDeviceListViewModel { Devices = devices.ToList() });
  }

  public AdminController(
    ISession session,
    IAuthorizationService authorizationService
  )
  {
    _session = session;
    _authorizationService = authorizationService;
  }

  private readonly ISession _session;

  private readonly IAuthorizationService _authorizationService;
}
