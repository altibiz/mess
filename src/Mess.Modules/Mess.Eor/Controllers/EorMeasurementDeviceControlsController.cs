using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;
using Mess.Eor.Extensions;
using Mess.OrchardCore;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;

namespace Mess.Eor.Controllers;

[Authorize]
public class EorMeasurementDeviceControlsController : Controller
{
  [HttpPost]
  public async Task<IActionResult> ToggleRunState(
    string contentItemId,
    [FromQuery] string? returnUrl
  )
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
      !await _authorizationService.AuthorizeControlAsync(
        User,
        orchardCoreUser,
        eorMeasurementDevice
      )
    )
    {
      return Forbid();
    }

    eorMeasurementDevice.Alter(
      eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
      eorMeasurementDevice =>
      {
        eorMeasurementDevice.Controls.RunState =
          eorMeasurementDevice.Controls.RunState
          == EorMeasurementDeviceRunState.Stopped
            ? EorMeasurementDeviceRunState.Started
            : EorMeasurementDeviceRunState.Stopped;
      }
    );
    await _contentManager.UpdateAsync(eorMeasurementDevice);

    if (returnUrl is not null)
    {
      return Redirect(returnUrl);
    }

    return RedirectToAction(
      nameof(EorMeasurementDeviceController.Detail),
      nameof(EorMeasurementDeviceController),
      new { contentItemId = contentItemId }
    );
  }

  [HttpPost]
  public async Task<IActionResult> Start(
    string contentItemId,
    [FromQuery] string? returnUrl
  )
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
      !await _authorizationService.AuthorizeControlAsync(
        User,
        orchardCoreUser,
        eorMeasurementDevice
      )
    )
    {
      return Forbid();
    }

    eorMeasurementDevice.Alter(
      eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
      eorMeasurementDevice =>
      {
        eorMeasurementDevice.Controls.RunState =
          EorMeasurementDeviceRunState.Started;
      }
    );
    await _contentManager.UpdateAsync(eorMeasurementDevice);

    if (returnUrl is not null)
    {
      return Redirect(returnUrl);
    }

    return RedirectToAction(
      nameof(EorMeasurementDeviceController.Detail),
      nameof(EorMeasurementDeviceController),
      new { contentItemId = contentItemId }
    );
  }

  [HttpPost]
  public async Task<IActionResult> Stop(
    string contentItemId,
    [FromQuery] string? returnUrl
  )
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
      !await _authorizationService.AuthorizeControlAsync(
        User,
        orchardCoreUser,
        eorMeasurementDevice
      )
    )
    {
      return Forbid();
    }

    eorMeasurementDevice.Alter(
      eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
      eorMeasurementDevice =>
      {
        eorMeasurementDevice.Controls.RunState =
          EorMeasurementDeviceRunState.Stopped;
      }
    );
    await _contentManager.UpdateAsync(eorMeasurementDevice);

    if (returnUrl is not null)
    {
      return Redirect(returnUrl);
    }

    return RedirectToAction(
      nameof(EorMeasurementDeviceController.Detail),
      nameof(EorMeasurementDeviceController),
      new { contentItemId = contentItemId }
    );
  }

  [HttpPost]
  public async Task<IActionResult> Reset(
    string contentItemId,
    [FromQuery] string? returnUrl
  )
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
      !await _authorizationService.AuthorizeControlAsync(
        User,
        orchardCoreUser,
        eorMeasurementDevice
      )
    )
    {
      return Forbid();
    }

    eorMeasurementDevice.Alter(
      eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
      eorMeasurementDevice =>
      {
        eorMeasurementDevice.Controls.ResetState =
          EorMeasurementDeviceResetState.ShouldntReset;
        eorMeasurementDevice.Controls.RunState =
          EorMeasurementDeviceRunState.Stopped;
      }
    );
    await _contentManager.UpdateAsync(eorMeasurementDevice);

    if (returnUrl is not null)
    {
      return Redirect(returnUrl);
    }

    return RedirectToAction(
      nameof(EorMeasurementDeviceController.Detail),
      nameof(EorMeasurementDeviceController),
      new { contentItemId = contentItemId }
    );
  }

  [HttpPost]
  public async Task<IActionResult> SetMode(
    string contentItemId,
    [FromQuery] string? returnUrl,
    [FromForm] int mode
  )
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
      !await _authorizationService.AuthorizeControlAsync(
        User,
        orchardCoreUser,
        eorMeasurementDevice
      )
    )
    {
      return Forbid();
    }

    eorMeasurementDevice.Alter(
      eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
      eorMeasurementDevice =>
      {
        eorMeasurementDevice.Controls.Mode = mode;
      }
    );
    await _contentManager.UpdateAsync(eorMeasurementDevice);

    if (returnUrl is not null)
    {
      return Redirect(returnUrl);
    }

    return RedirectToAction(
      nameof(EorMeasurementDeviceController.Detail),
      nameof(EorMeasurementDeviceController),
      new { contentItemId = contentItemId }
    );
  }

  public EorMeasurementDeviceControlsController(
    IAuthorizationService authorizationService,
    IContentManager contentManager
  )
  {
    _contentManager = contentManager;
    _authorizationService = authorizationService;
  }

  private readonly IContentManager _contentManager;
  private readonly IAuthorizationService _authorizationService;
}
