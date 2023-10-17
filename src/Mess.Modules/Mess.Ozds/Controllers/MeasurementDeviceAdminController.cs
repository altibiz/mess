using Mess.Ozds.Abstractions.Indexes;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using YesSql;
using Mess.Ozds.ViewModels;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.Title.Models;
using Mess.Iot.Abstractions.Models;
using Mess.Iot.Abstractions.Indexes;
using OrchardCore.Admin;

namespace Mess.Ozds.Controllers;

[Admin]
public class MeasurementDeviceAdminController : Controller
{
  public async Task<IActionResult> List()
  {
    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    IEnumerable<ContentItem>? contentItems = null;
    if (orchardCoreUser.RoleNames.Contains("Administrator"))
    {
      contentItems = await _session
        .Query<ContentItem, IotDeviceIndex>()
        .ListAsync();
    }
    else if (
      orchardCoreUser.RoleNames.Contains(
        "DistributionSystemOperatorRepresentative"
      )
    )
    {
      contentItems = await _session
        .Query<
          ContentItem,
          OzdsMeasurementDeviceDistributionSystemOperatorIndex
        >()
        .Where(
          index =>
            index.DistributionSystemOperatorRepresentativeUserId
            == orchardCoreUser.UserId
        )
        .ListAsync();
    }
    else if (
      orchardCoreUser.RoleNames.Contains(
        "ClosedDistributionSystemRepresentative"
      )
    )
    {
      contentItems = await _session
        .Query<
          ContentItem,
          OzdsMeasurementDeviceClosedDistributionSystemIndex
        >()
        .Where(
          index =>
            index.ClosedDistributionSystemRepresentativeUserId
            == orchardCoreUser.UserId
        )
        .ListAsync();
    }
    else if (
      orchardCoreUser.RoleNames.Contains("DistributionSystemUnitRepresentative")
    )
    {
      contentItems = await _session
        .Query<ContentItem, OzdsMeasurementDeviceDistributionSystemUnitIndex>()
        .Where(
          index =>
            index.DistributionSystemUnitRepresentativeUserId
            == orchardCoreUser.UserId
        )
        .ListAsync();
    }
    else
    {
      return Forbid();
    }

    return View(
      new OzdsMeasurementDeviceListViewModel
      {
        ContentItems = contentItems
          .Select(
            contentItem =>
              (
                ContentItem: contentItem,
                TitlePart: contentItem.As<TitlePart>(),
                OzdsMeasurementDevicePart: contentItem.As<OzdsMeasurementDevicePart>(),
                MeasurementDevicePart: contentItem.As<IotDevicePart>()
              )
          )
          .ToList()
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

    var ozdsMeasurementDevicePart = contentItem.As<OzdsMeasurementDevicePart>();
    if (ozdsMeasurementDevicePart == null)
    {
      return NotFound();
    }

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    if (
      !orchardCoreUser.RoleNames.Contains("Administrator")
      && !(
        orchardCoreUser.RoleNames.Contains(
          "DistributionSystemOperatorRepresentative"
        )
        && ozdsMeasurementDevicePart.DistributionSystemOperatorRepresentativeUserIds.Contains(
          orchardCoreUser.UserId
        )
      )
    )
    {
      return Forbid();
    }

    return View(
      new OzdsMeasurementDeviceDetailViewModel
      {
        ContentItem = contentItem,
        TitlePart = contentItem.As<TitlePart>(),
        OzdsMeasurementDevicePart = contentItem.As<OzdsMeasurementDevicePart>(),
        MeasurementDevicePart = contentItem.As<IotDevicePart>()
      }
    );
  }

  public MeasurementDeviceAdminController(
    IAuthorizationService authorizationService,
    IContentManager contentManager,
    ISession session
  )
  {
    _contentManager = contentManager;
    _authorizationService = authorizationService;
    _session = session;
  }

  private readonly IContentManager _contentManager;
  private readonly IAuthorizationService _authorizationService;
  private readonly ISession _session;
}
