using Mess.Ozds.Abstractions.Indexes;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using YesSql;
using Mess.Ozds.ViewModels;
using Mess.Ozds.Abstractions.Models;

namespace Mess.Ozds.Controllers;

[Authorize]
public class OzdsMeasurementDeviceController : Controller
{
  public async Task<IActionResult> List()
  {
    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    IEnumerable<ContentItem>? contentItems = null;
    if (
      orchardCoreUser.RoleNames.Contains(
        "DistributionSystemOperatorRepresentative"
      )
    )
    {
      contentItems = await _session
        .Query<ContentItem, OzdsMeasurementDeviceIndex>()
        .Where(
          index =>
            index.DistributionSystemOperatorRepresentativeUserIds.Contains(
              orchardCoreUser.UserId
            )
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
        .Query<ContentItem, OzdsMeasurementDeviceIndex>()
        .Where(
          index =>
            index.ClosedDistributionSystemRepresentativeUserIds.Contains(
              orchardCoreUser.UserId
            )
        )
        .ListAsync();
    }
    else if (
      orchardCoreUser.RoleNames.Contains("DistributionSystemUnitRepresentative")
    )
    {
      contentItems = await _session
        .Query<ContentItem, OzdsMeasurementDeviceIndex>()
        .Where(
          index =>
            index.DistributionSystemUnitRepresentativeUserIds.Contains(
              orchardCoreUser.UserId
            )
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
        ContentItems = contentItems.ToList(),
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
      !orchardCoreUser.RoleNames.Contains(
        "DistributionSystemOperatorRepresentative"
      )
      || !ozdsMeasurementDevicePart.DistributionSystemOperatorRepresentativeUserIds.Contains(
        orchardCoreUser.UserId
      )
    )
    {
      return Forbid();
    }

    return View(
      new OzdsMeasurementDeviceDetailViewModel { ContentItem = contentItem, }
    );
  }

  public OzdsMeasurementDeviceController(
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
