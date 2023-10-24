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
using OrchardCore.ContentFields.Indexing.SQL;

namespace Mess.Ozds.Controllers;

[Admin]
public class OzdsIotDeviceController : Controller
{
  public async Task<IActionResult> List()
  {
    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var legalEntityItem = await _session
      .Query<ContentItem, UserPickerFieldIndex>()
      .Where(index => index.ContentPart == "LegalEntityPart")
      .Where(index => index.SelectedUserId == orchardCoreUser.UserId)
      .FirstOrDefaultAsync();

    IEnumerable<ContentItem>? devices = null;
    if (orchardCoreUser.RoleNames.Contains("Administrator"))
    {
      devices = await _session
        .Query<ContentItem, OzdsIotDeviceIndex>()
        .ListAsync();
    }
    else if (
      orchardCoreUser.RoleNames.Contains(
        "DistributionSystemOperatorRepresentative"
      )
    )
    {
      devices = await _session
        .Query<ContentItem, OzdsIotDeviceIndex>()
        .Where(
          index =>
            index.DistributionSystemOperatorContentItemId
            == legalEntityItem.ContentItemId
        )
        .ListAsync();
    }
    else if (
      orchardCoreUser.RoleNames.Contains(
        "ClosedDistributionSystemRepresentative"
      )
    )
    {
      devices = await _session
        .Query<ContentItem, OzdsIotDeviceIndex>()
        .Where(
          index =>
            index.ClosedDistributionSystemContentItemId
            == legalEntityItem.ContentItemId
        )
        .ListAsync();
    }
    else if (
      orchardCoreUser.RoleNames.Contains("DistributionSystemUnitRepresentative")
    )
    {
      devices = await _session
        .Query<ContentItem, OzdsIotDeviceIndex>()
        .Where(
          index =>
            index.DistributionSystemUnitContentItemId
            == legalEntityItem.ContentItemId
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
        ContentItems = devices
          .Select(
            device =>
              (
                ContentItem: device,
                OzdsIotDevicePart: device.As<OzdsIotDevicePart>(),
                IotDevicePart: device.As<IotDevicePart>()
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
    var iotDevicePart = contentItem.As<IotDevicePart>();
    if (ozdsIotDevicePart is null || iotDevicePart is null)
    {
      return NotFound();
    }

    var index = await _session
      .QueryIndex<OzdsIotDeviceIndex>()
      .Where(index => index.OzdsIotDeviceContentItemId == contentItemId)
      .FirstOrDefaultAsync();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var legalEntityItem = await _session
      .Query<ContentItem, UserPickerFieldIndex>()
      .Where(index => index.ContentPart == "LegalEntityPart")
      .Where(index => index.SelectedUserId == orchardCoreUser.UserId)
      .FirstOrDefaultAsync();
    if (
      !orchardCoreUser.RoleNames.Contains("Administrator")
      && !(
        orchardCoreUser.RoleNames.Contains(
          "DistributionSystemOperatorRepresentative"
        )
        && index is not null
        && legalEntityItem is not null
        && index.DistributionSystemOperatorContentItemId
          == legalEntityItem.ContentItemId
      )
      && !(
        orchardCoreUser.RoleNames.Contains(
          "ClosedDistributionSystemRepresentative"
        )
        && index is not null
        && legalEntityItem is not null
        && index.ClosedDistributionSystemContentItemId
          == legalEntityItem.ContentItemId
      )
      && !(
        orchardCoreUser.RoleNames.Contains(
          "DistributionSystemUnitRepresentative"
        )
        && index is not null
        && legalEntityItem is not null
        && index.DistributionSystemUnitContentItemId
          == legalEntityItem.ContentItemId
      )
    )
    {
      return Forbid();
    }

    return View(
      new OzdsIotDeviceDetailViewModel
      {
        ContentItem = contentItem,
        OzdsIotDevicePart = ozdsIotDevicePart,
        IotDevicePart = iotDevicePart
      }
    );
  }

  public OzdsIotDeviceController(
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
