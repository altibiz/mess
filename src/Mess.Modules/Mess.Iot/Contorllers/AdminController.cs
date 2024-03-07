using Mess.Cms.Extensions.OrchardCore;
using Mess.Iot.Abstractions;
using Mess.Iot.Abstractions.Indexes;
using Mess.Iot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Iot.Controllers;

[Admin]
public class AdminController : Controller
{
  private readonly IAuthorizationService _authorizationService;

  private readonly ISession _session;

  public AdminController(
    ISession session,
    IAuthorizationService authorizationService
  )
  {
    _session = session;
    _authorizationService = authorizationService;
  }

  public async Task<IActionResult> ListIotDevices()
  {
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        Permissions.ListIotDevices
      )
    )
      return Unauthorized();

    var devices = await _session
      .Query<ContentItem, IotDeviceIndex>()
      .LatestPublished()
      .ListAsync();

    return View(new IotDeviceListViewModel { Devices = devices.ToList() });
  }
}
