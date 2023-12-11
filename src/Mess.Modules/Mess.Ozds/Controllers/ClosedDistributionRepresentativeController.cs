using Mess.Blazor.Abstractions.Controllers;
using Mess.Cms.Extensions.Microsoft;
using Mess.Iot.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Components.ClosedDistributionSystemRepresentative;
using Mess.Ozds.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Ozds.Controllers;

public class ClosedDistributionSystemRepresentativeController : Controller
{
  private readonly ISession _session;

  public ClosedDistributionSystemRepresentativeController(
    ISession session
  )
  {
    _session = session;
  }

  public async Task<IActionResult> Dashboard()
  {
    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();

    var legalEntityItem = await _session
      .Query<ContentItem, UserPickerFieldIndex>()
      .Where(index => index.ContentPart == "LegalEntityPart")
      .Where(index => index.SelectedUserId == orchardCoreUser.UserId)
      .FirstOrDefaultAsync();

    IEnumerable<ContentItem>? devices = null;
    if (
      orchardCoreUser.RoleNames.Contains(
        "ClosedDistributionSystemRepresentative"
      )
    )
      devices = await _session
        .Query<ContentItem, OzdsIotDeviceIndex>()
        .Where(
          index =>
            index.ClosedDistributionSystemContentItemId
            == legalEntityItem.ContentItemId
        )
        .ListAsync();
    else
      return Forbid();

    return await this.Component(
      typeof(Dashboard),
      new ClosedDistributionSystemRepresentativeDashboardViewModel
      {
        Devices = devices
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
}
