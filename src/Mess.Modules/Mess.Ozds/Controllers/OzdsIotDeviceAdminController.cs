using Mess.Cms.Extensions.Microsoft;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Ozds.Controllers;

[Admin]
public class OzdsIotDeviceAdminController : Controller
{
  private readonly IContentManager _contentManager;
  private readonly ISession _session;

  public OzdsIotDeviceAdminController(
    IContentManager contentManager,
    ISession session
  )
  {
    _contentManager = contentManager;
    _session = session;
  }

  public async Task<IActionResult> List()
  {
    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var legalEntityItem = await _session
      .Query<ContentItem, UserPickerFieldIndex>()
      .Where(index => index.ContentPart == "LegalEntityPart")
      .Where(index => index.SelectedUserId == orchardCoreUser.UserId)
      .LatestPublished()
      .FirstOrDefaultAsync();

    IEnumerable<ContentItem>? devices = null;
    if (orchardCoreUser.RoleNames.Contains("Administrator"))
      devices = await _session
        .Query<ContentItem, OzdsIotDeviceIndex>()
        .LatestPublished()
        .ListAsync();
    else if (
      orchardCoreUser.RoleNames.Contains(
        "Distribution System Operator Representative"
      )
    )
      devices = await _session
        .Query<ContentItem, OzdsIotDeviceIndex>()
        .Where(
          index =>
            index.DistributionSystemOperatorContentItemId
            == legalEntityItem.ContentItemId
        )
        .LatestPublished()
        .ListAsync();
    else if (
      orchardCoreUser.RoleNames.Contains(
        "Closed Distribution System Representative"
      )
    )
      devices = await _session
        .Query<ContentItem, OzdsIotDeviceIndex>()
        .Where(
          index =>
            index.ClosedDistributionSystemContentItemId
            == legalEntityItem.ContentItemId
        )
        .LatestPublished()
        .ListAsync();
    else if (
      orchardCoreUser.RoleNames.Contains("Distribution System Unit Representative")
    )
      devices = await _session
        .Query<ContentItem, OzdsIotDeviceIndex>()
        .Where(
          index =>
            index.DistributionSystemUnitContentItemId
            == legalEntityItem.ContentItemId
        )
        .LatestPublished()
        .ListAsync();
    else
      return Forbid();

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
    if (contentItem == null) return NotFound();

    var ozdsIotDevicePart = contentItem.As<OzdsIotDevicePart>();
    var iotDevicePart = contentItem.As<IotDevicePart>();
    if (ozdsIotDevicePart is null || iotDevicePart is null) return NotFound();

    var index = await _session
      .QueryIndex<OzdsIotDeviceIndex>()
      .Where(index => index.OzdsIotDeviceContentItemId == contentItemId)
      .FirstOrDefaultAsync();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var legalEntityItem = await _session
      .Query<ContentItem, UserPickerFieldIndex>()
      .Where(index => index.ContentPart == "LegalEntityPart")
      .Where(index => index.SelectedUserId == orchardCoreUser.UserId)
      .LatestPublished()
      .FirstOrDefaultAsync();

    return !orchardCoreUser.RoleNames.Contains("Administrator")
           && !(
             orchardCoreUser.RoleNames.Contains(
               "Distribution System Operator Representative"
             )
             && index is not null
             && legalEntityItem is not null
             && index.DistributionSystemOperatorContentItemId
             == legalEntityItem.ContentItemId
           )
           && !(
             orchardCoreUser.RoleNames.Contains(
               "Closed Distribution System Representative"
             )
             && index is not null
             && legalEntityItem is not null
             && index.ClosedDistributionSystemContentItemId
             == legalEntityItem.ContentItemId
           )
           && !(
             orchardCoreUser.RoleNames.Contains(
               "Distribution System Unit Representative"
             )
             && index is not null
             && legalEntityItem is not null
             && index.DistributionSystemUnitContentItemId
             == legalEntityItem.ContentItemId
           )
      ? Forbid()
      : View(
        new OzdsIotDeviceDetailViewModel
        {
          ContentItem = contentItem,
          OzdsIotDevicePart = ozdsIotDevicePart,
          IotDevicePart = iotDevicePart
        }
      );
  }
}
