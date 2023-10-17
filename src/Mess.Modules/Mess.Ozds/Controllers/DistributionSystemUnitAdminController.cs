using Mess.Ozds.Abstractions.Timeseries;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.ViewModels;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using YesSql;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.Title.Models;
using Mess.Iot.Abstractions.Models;
using Mess.Iot.Abstractions.Indexes;

namespace Mess.Ozds.Controllers;

[Admin]
public class DistributionSystemUnitAdminController : Controller
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
          OzdsIotDeviceDistributionSystemOperatorIndex
        >()
        .Where(
          index =>
            index.DistributionSystemOperatorRepresentativeUserId
            == orchardCoreUser.UserId
        )
        .ListAsync();
    }
    else
    {
      return Forbid();
    }

    return View(
      new OzdsIotDeviceListViewModel
      {
        ContentItems = contentItems
          .Select(
            contentItem =>
              (
                ContentItem: contentItem,
                TitlePart: contentItem.As<TitlePart>(),
                OzdsIotDevicePart: contentItem.As<OzdsIotDevicePart>(),
                IotDevicePart: contentItem.As<IotDevicePart>()
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

    var ozdsIotDevicePart = contentItem.As<OzdsIotDevicePart>();
    if (ozdsIotDevicePart == null)
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
        && ozdsIotDevicePart.DistributionSystemOperatorRepresentativeUserIds.Contains(
          orchardCoreUser.UserId
        )
      )
    )
    {
      return Forbid();
    }

    return View(
      new OzdsIotDeviceDetailViewModel
      {
        ContentItem = contentItem,
        TitlePart = contentItem.As<TitlePart>(),
        OzdsIotDevicePart = contentItem.As<OzdsIotDevicePart>(),
        IotDevicePart = contentItem.As<IotDevicePart>()
      }
    );
  }

  public DistributionSystemUnitAdminController(
    IAuthorizationService authorizationService,
    IContentManager contentManager,
    ISession session,
    IOzdsTimeseriesClient measurementClient
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
  private readonly IOzdsTimeseriesClient _measurementClient;
}
