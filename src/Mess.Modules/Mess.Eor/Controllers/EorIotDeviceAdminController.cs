using Mess.Cms;
using Mess.Cms.Extensions.Microsoft;
using Mess.Eor.Abstractions.Indexes;
using Mess.Eor.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Extensions;
using Mess.Eor.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Contents;
using YesSql;

namespace Mess.Eor.Controllers;

[Admin]
[Authorize]
public class EorIotDeviceAdminController : Controller
{
  private readonly IAuthorizationService _authorizationService;

  private readonly IContentManager _contentManager;
  private readonly IEorTimeseriesClient _measurementClient;
  private readonly ISession _session;

  public EorIotDeviceAdminController(
    IAuthorizationService authorizationService,
    IContentManager contentManager,
    ISession session,
    IEorTimeseriesClient measurementClient
  )
  {
    _contentManager = contentManager;
    _authorizationService = authorizationService;
    _session = session;
    _measurementClient = measurementClient;
  }

  public async Task<IActionResult> List()
  {
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        CommonPermissions.ViewOwnContent,
        (object)"EorIotDevice"
      )
    )
      return Forbid();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var contentItems = await _session
      .Query<ContentItem, EorIotDeviceIndex>()
      .Where(index => index.Author == orchardCoreUser.UserId)
      .ListAsync();

    var eorIotDevices = contentItems.Select(
      contentItem => contentItem.AsContent<EorIotDeviceItem>()
    );

    var eorIotDeviceSummaries =
      await _measurementClient.GetEorIotDeviceSummariesAsync(
        eorIotDevices
          .Select(
            eorIotDevice => eorIotDevice.IotDevicePart.Value.DeviceId.Text
          )
          .ToList()
      );

    return View(
      new EorIotDeviceListViewModel
      {
        EorIotDevices = eorIotDevices
          .Select(
            eorIotDevice =>
            (
              eorIotDevice,
              eorIotDeviceSummaries.FirstOrDefault(
                summary =>
                  summary.DeviceId
                  == eorIotDevice.IotDevicePart.Value.DeviceId.Text
              )
            )
          )
          .ToList()
      }
    );
  }

  public async Task<IActionResult> Detail(string contentItemId)
  {
    var contentItem = await _contentManager.GetAsync(contentItemId);
    if (contentItem == null) return NotFound();

    var eorIotDevice = contentItem.AsContent<EorIotDeviceItem>();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    if (
      !await _authorizationService.AuthorizeViewAsync(
        User,
        orchardCoreUser,
        eorIotDevice
      )
    )
      return Forbid();

    var eorIotDeviceSummary =
      await _measurementClient.GetEorIotDeviceSummaryAsync(
        eorIotDevice.IotDevicePart.Value.DeviceId.Text
      );

    return eorIotDeviceSummary == null
      ? NotFound()
      : View(
        new EorIotDeviceDetailViewModel
        {
          EorIotDeviceItem = eorIotDevice,
          EorSummary = eorIotDeviceSummary
        }
      );
  }
}
