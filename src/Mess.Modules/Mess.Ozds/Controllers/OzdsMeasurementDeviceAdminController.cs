using Mess.Ozds.Abstractions.Client;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.ViewModels;
using Mess.OrchardCore;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Contents;
using YesSql;

namespace Mess.Ozds.Controllers;

[Admin]
[Authorize]
public class OzdsMeasurementDeviceAdminController : Controller
{
  public async Task<IActionResult> List()
  {
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        CommonPermissions.ViewOwnContent,
        (object)"OzdsMeasurementDevice"
      )
    )
    {
      return Forbid();
    }

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var contentItems = await _session
      .Query<ContentItem, OzdsMeasurementDeviceIndex>()
      .Where(index => index.Author == orchardCoreUser.UserId)
      .ListAsync();

    var ozdsMeasurementDevices = contentItems.Select(
      contentItem => contentItem.AsContent<OzdsMeasurementDeviceItem>()
    );

    var ozdsMeasurementDeviceSummaries =
      await _measurementClient.GetOzdsMeasurementDeviceSummariesAsync(
        ozdsMeasurementDevices
          .Select(
            ozdsMeasurementDevice =>
              ozdsMeasurementDevice.MeasurementDevicePart.Value.DeviceId.Text
          )
          .ToList()
      );

    return View(
      new OzdsMeasurementDeviceListViewModel
      {
        OzdsMeasurementDevices = ozdsMeasurementDevices
          .Select(
            ozdsMeasurementDevice =>
              (
                ozdsMeasurementDevice,
                ozdsMeasurementDeviceSummaries.FirstOrDefault(
                  summary =>
                    summary.DeviceId
                    == ozdsMeasurementDevice
                      .MeasurementDevicePart
                      .Value
                      .DeviceId
                      .Text
                )
              )
          )
          .ToList(),
      }
    );
  }

  public async Task<IActionResult> Detail(string contentItemId)
  {
    var contentItem = await _contentManager.GetAsync(contentItemId);
    if (contentItem == null)
    {
      return NotFound();
    }

    var ozdsMeasurementDevice =
      contentItem.AsContent<OzdsMeasurementDeviceItem>();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    if (
      !await _authorizationService.AuthorizeViewAsync(
        User,
        orchardCoreUser,
        ozdsMeasurementDevice
      )
    )
    {
      return Forbid();
    }

    var ozdsMeasurementDeviceSummary =
      await _measurementClient.GetOzdsMeasurementDeviceSummaryAsync(
        ozdsMeasurementDevice.MeasurementDevicePart.Value.DeviceId.Text
      );
    if (ozdsMeasurementDeviceSummary == null)
    {
      return NotFound();
    }

    return View(
      new OzdsMeasurementDeviceDetailViewModel
      {
        OzdsMeasurementDevice = ozdsMeasurementDevice,
        OzdsMeasurementDeviceSummary = ozdsMeasurementDeviceSummary
      }
    );
  }

  public OzdsMeasurementDeviceAdminController(
    IAuthorizationService authorizationService,
    IContentManager contentManager,
    ISession session,
    IOzdsClient measurementClient
  )
  {
    _contentManager = contentManager;
    _authorizationService = authorizationService;
    _session = session;
    _measurementClient = measurementClient;
  }

  private readonly IContentManager _contentManager;
  private readonly IAuthorizationService _authorizationService;
  private readonly ISession _session;
  private readonly IOzdsClient _measurementClient;
}
