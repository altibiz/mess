using System.Text.Json;
using Mess.Eor.Abstractions;
using Mess.Eor.Abstractions.Client;
using Mess.Eor.Abstractions.Models;
using Mess.OrchardCore;
using Mess.System.Extensions.Json;
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
    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        EorPermissions.ViewEorMeasurementDevice
      )
    )
    {
      return Forbid();
    }

    var contentItem = await _contentManager.GetAsync(contentItemId);
    if (contentItem == null)
    {
      return NotFound();
    }
    var eorMeasurementDevice =
      contentItem.AsContent<EorMeasurementDeviceItem>();

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
