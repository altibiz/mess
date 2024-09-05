using System.Globalization;
using Mess.Cms;
using Mess.Cms.Extensions.Microsoft;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Eor.Abstractions;
using Mess.Eor.Abstractions.Indexes;
using Mess.Eor.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Extensions;
using Mess.Eor.Localization.Abstractions;
using Mess.Eor.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.Contents;
using YesSql;

namespace Mess.Eor.Controllers;

[Authorize]
public class EorIotDeviceController : Controller
{
  private readonly IAuthorizationService _authorizationService;

  private readonly IContentManager _contentManager;
  private readonly IEorTimeseriesQuery _measurementQuery;
  private readonly ISession _session;
  private readonly IEorLocalizer _localizer;

  public EorIotDeviceController(
    IAuthorizationService authorizationService,
    IContentManager contentManager,
    ISession session,
    IEorTimeseriesQuery measurementQuery,
    IEorLocalizer localizer
  )
  {
    _contentManager = contentManager;
    _authorizationService = authorizationService;
    _session = session;
    _measurementQuery = measurementQuery;
    _localizer = localizer;
  }

  public async Task<IActionResult> List(string culture)
  {
    if (!string.IsNullOrEmpty(culture))
    {
      var cultureInfo = new CultureInfo(culture);
      CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
      CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    }

    var canViewOwned = await _authorizationService.AuthorizeAsync(
        User,
        EorPermissions.ViewOwnedEorIotDevices
    );
    var canViewOwn = await _authorizationService.AuthorizeAsync(
        User,
        CommonPermissions.ViewOwnContent,
        (object)"EorIotDevice"
    );
    if (!(canViewOwned || canViewOwn)) return Forbid();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var contentItems = await _session
        .Query<ContentItem, EorIotDeviceIndex>()
        .Where(
            canViewOwned
                ? index => index.OwnerId == orchardCoreUser.UserId
                : index => index.Author == orchardCoreUser.UserId
        )
        .LatestPublished()
        .ListAsync();

    var eorIotDevices = contentItems.Select(
        contentItem => contentItem.AsContent<EorIotDeviceItem>()
    );

    var eorIotDeviceSummaries =
        await _measurementQuery.GetEorIotDeviceSummariesAsync(
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
                .ToList(),
          Culture = culture
        }
    );
  }

  public async Task<IActionResult> Detail(string contentItemId, string culture)
  {
    if (!string.IsNullOrEmpty(culture))
    {
      var cultureInfo = new CultureInfo(culture);
      CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
      CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    }

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
        await _measurementQuery.GetEorIotDeviceSummaryAsync(
            eorIotDevice.IotDevicePart.Value.DeviceId.Text
        );

    return eorIotDeviceSummary == null
        ? NotFound()
        : View(
            new EorIotDeviceDetailViewModel
            {
              EorIotDeviceItem = eorIotDevice,
              EorSummary = eorIotDeviceSummary,
              Culture = culture
            }
        );
  }
}
