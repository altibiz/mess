using Mess.Ozds.Abstractions;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using Mess.OrchardCore;
using YesSql;
using Mess.Ozds.ViewModels;
using Mess.Ozds.Abstractions.Client;
using Mess.Ozds.Extensions;
using OrchardCore.Contents;

namespace Mess.Ozds.Controllers;

[Authorize]
public class OzdsMeasurementDeviceController : Controller
{
  public async Task<IActionResult> List()
  {
    var canViewOwned = await _authorizationService.AuthorizeAsync(
      User,
      OzdsPermissions.ViewOwnedozdsMeasurementDevices
    );
    // TODO: fix returns true for owners??
    var canViewOwn = await _authorizationService.AuthorizeAsync(
      User,
      CommonPermissions.ViewOwnContent,
      (object)"OzdsMeasurementDevice"
    );
    if (!(canViewOwned || canViewOwn))
    {
      return Forbid();
    }

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var contentItems = await _session
      .Query<ContentItem, OzdsMeasurementDeviceIndex>()
      .Where(
        canViewOwned
          ? index => index.OwnerId == orchardCoreUser.UserId
          : index => index.Author == orchardCoreUser.UserId
      )
      .ListAsync();

    var ozdsMeasurementDevices = contentItems.Select(
      contentItem => contentItem.AsContent<OzdsMeasurementDeviceItem>()
    );

    var ozdsMeasurementDeviceSummaries =
      await _measurementQuery.GetOzdsMeasurementDeviceSummariesAsync(
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
      await _measurementQuery.GetOzdsMeasurementDeviceSummaryAsync(
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

  public OzdsMeasurementDeviceController(
    IAuthorizationService authorizationService,
    IContentManager contentManager,
    ISession session,
    IOzdsTimeseriesQuery measurementQuery
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
  private readonly IOzdsTimeseriesQuery _measurementQuery;
}
