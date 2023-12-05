using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Extensions;
using Mess.Cms;
using Mess.Cms.Extensions.Microsoft;
using Mess.Eor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.Controllers;

[Authorize]
public class EorIotDeviceDataController : Controller
{
  [HttpGet]
  [IgnoreAntiforgeryToken]
  public async Task<IActionResult> Index(string contentItemId)
  {
    var contentItem = await _contentManager.GetAsync(contentItemId);
    if (contentItem == null)
    {
      return NotFound();
    }

    var eorIotDevice = contentItem.AsContent<EorIotDeviceItem>();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    if (
      !await _authorizationService.AuthorizeViewAsync(
        User,
        orchardCoreUser,
        eorIotDevice
      )
    )
    {
      return Forbid();
    }

    var eorIotDeviceSummary =
      await _measurementClient.GetEorIotDeviceSummaryAsync(
        eorIotDevice.IotDevicePart.Value.DeviceId.Text
      );

    return eorIotDeviceSummary == null
      ? NotFound()
      : Json(
      new EorIotDeviceDataModel
      {
        Controls = eorIotDevice.EorIotDevicePart.Value.Controls,
        Summary = eorIotDeviceSummary
      }
    );
  }

  public EorIotDeviceDataController(
    IAuthorizationService authorizationService,
    IContentManager contentManager,
    IEorTimeseriesClient measurementClient
  )
  {
    _contentManager = contentManager;
    _authorizationService = authorizationService;
    _measurementClient = measurementClient;
  }

  private readonly IContentManager _contentManager;
  private readonly IAuthorizationService _authorizationService;
  private readonly IEorTimeseriesClient _measurementClient;
}
