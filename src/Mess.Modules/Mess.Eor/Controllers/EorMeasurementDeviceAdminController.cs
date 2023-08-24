using Mess.Eor.Abstractions;
using Mess.Eor.Abstractions.Client;
using Mess.Eor.Abstractions.Indexes;
using Mess.Eor.Abstractions.Models;
using Mess.Eor.Extensions;
using Mess.Eor.ViewModels;
using Mess.OrchardCore;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Contents;
using YesSql;

namespace Mess.Eor.Controllers;

[Admin]
[Authorize]
public class EorMeasurementDeviceAdminController : Controller
{
  public async Task<IActionResult> List()
  {
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        CommonPermissions.ViewOwnContent,
        (object)"EorMeasurementDevice"
      )
    )
    {
      return Forbid();
    }

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var contentItems = await _session
      .Query<ContentItem, EorMeasurementDeviceIndex>()
      .Where(index => index.Author == orchardCoreUser.UserId)
      .ListAsync();

    var eorMeasurementDevices = contentItems.Select(
      contentItem => contentItem.AsContent<EorMeasurementDeviceItem>()
    );

    var eorMeasurementDeviceSummaries =
      await _measurementClient.GetEorMeasurementDeviceSummariesAsync(
        eorMeasurementDevices
          .Select(
            eorMeasurementDevice =>
              eorMeasurementDevice.MeasurementDevicePart.Value.DeviceId.Text
          )
          .ToList()
      );

    return View(
      new EorMeasurementDeviceListViewModel
      {
        EorMeasurementDevices = eorMeasurementDevices
          .Select(
            eorMeasurementDevice =>
              (
                eorMeasurementDevice,
                eorMeasurementDeviceSummaries.FirstOrDefault(
                  summary =>
                    summary.DeviceId
                    == eorMeasurementDevice
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

    var eorMeasurementDevice =
      contentItem.AsContent<EorMeasurementDeviceItem>();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    if (
      !await _authorizationService.AuthorizeViewAsync(
        User,
        orchardCoreUser,
        eorMeasurementDevice
      )
    )
    {
      return Forbid();
    }

    var eorMeasurementDeviceSummary =
      await _measurementClient.GetEorMeasurementDeviceSummaryAsync(
        eorMeasurementDevice.MeasurementDevicePart.Value.DeviceId.Text
      );
    if (eorMeasurementDeviceSummary == null)
    {
      return NotFound();
    }

    return View(
      new EorMeasurementDeviceDetailViewModel
      {
        EorMeasurementDevice = eorMeasurementDevice,
        EorMeasurementDeviceSummary = eorMeasurementDeviceSummary
      }
    );
  }

  public EorMeasurementDeviceAdminController(
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

  private readonly IContentManager _contentManager;
  private readonly IAuthorizationService _authorizationService;
  private readonly ISession _session;
  private readonly IEorTimeseriesClient _measurementClient;
}
