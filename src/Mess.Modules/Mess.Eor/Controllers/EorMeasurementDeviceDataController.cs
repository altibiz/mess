using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;
using Mess.Eor.Extensions;
using Mess.OrchardCore;
using Mess.OrchardCore.Extensions.Microsoft;
using Mess.Eor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;

namespace Mess.Eor.Controllers;

[Authorize]
public class EorMeasurementDeviceDataController : Controller
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
      await _measurementClient.GetEorMeasurementDeviceSummaryAsync(
        eorMeasurementDevice.MeasurementDevicePart.Value.DeviceId.Text
      );
    if (eorMeasurementDeviceSummary == null)
    {
      return NotFound();
    }

    return Json(
      new EorMeasurementDeviceDataModel
      {
        EorMeasurementDeviceControls = eorMeasurementDevice
          .EorMeasurementDevicePart
          .Value
          .Controls,
        EorMeasurementDeviceSummary = eorMeasurementDeviceSummary
      }
    );
  }

  public EorMeasurementDeviceDataController(
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
