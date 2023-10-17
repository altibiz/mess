using Mess.Eor.Abstractions;
using Mess.Eor.Abstractions.Indexes;
using Mess.Eor.Abstractions.Models;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using Mess.OrchardCore;
using YesSql;
using Mess.Eor.ViewModels;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Extensions;
using OrchardCore.Contents;

namespace Mess.Eor.Controllers;

[Authorize]
public class EorMeasurementDeviceController : Controller
{
  public async Task<IActionResult> List()
  {
    var canViewOwned = await _authorizationService.AuthorizeAsync(
      User,
      EorPermissions.ViewOwnedEorMeasurementDevices
    );
    // TODO: fix returns true for owners??
    var canViewOwn = await _authorizationService.AuthorizeAsync(
      User,
      CommonPermissions.ViewOwnContent,
      (object)"EorMeasurementDevice"
    );
    if (!(canViewOwned || canViewOwn))
    {
      return Forbid();
    }

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var contentItems = await _session
      .Query<ContentItem, EorIotDeviceIndex>()
      .Where(
        canViewOwned
          ? index => index.OwnerId == orchardCoreUser.UserId
          : index => index.Author == orchardCoreUser.UserId
      )
      .ListAsync();

    var eorMeasurementDevices = contentItems.Select(
      contentItem => contentItem.AsContent<EorMeasurementDeviceItem>()
    );

    var eorMeasurementDeviceSummaries =
      await _measurementQuery.GetEorMeasurementDeviceSummariesAsync(
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
      await _measurementQuery.GetEorMeasurementDeviceSummaryAsync(
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

  public EorMeasurementDeviceController(
    IAuthorizationService authorizationService,
    IContentManager contentManager,
    ISession session,
    IEorTimeseriesQuery measurementQuery
  )
  {
    _contentManager = contentManager;
    _authorizationService = authorizationService;
    _session = session;
    _measurementQuery = measurementQuery;
  }

  private readonly IContentManager _contentManager;
  private readonly IAuthorizationService _authorizationService;
  private readonly ISession _session;
  private readonly IEorTimeseriesQuery _measurementQuery;
}
