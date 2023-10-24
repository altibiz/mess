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
public class EorIotDeviceControlsController : Controller
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

    var eorIotDevice = contentItem.AsContent<EorIotDeviceItem>();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    if (
      !await _authorizationService.AuthorizeControlAsync(
        User,
        orchardCoreUser,
        eorIotDevice
      )
    )
    {
      return Forbid();
    }

    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.EorIotDevicePart,
      eorIotDevice =>
      {
        eorIotDevice.Controls.RunState =
          eorIotDevice.Controls.RunState == EorRunState.Stopped
            ? EorRunState.Started
            : EorRunState.Stopped;
      }
    );
    await _contentManager.UpdateAsync(eorIotDevice);

    if (returnUrl is not null)
    {
      return Redirect(returnUrl);
    }

    return RedirectToAction(
      nameof(EorIotDeviceController.Detail),
      nameof(EorIotDeviceController),
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

    var eorIotDevice = contentItem.AsContent<EorIotDeviceItem>();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    if (
      !await _authorizationService.AuthorizeControlAsync(
        User,
        orchardCoreUser,
        eorIotDevice
      )
    )
    {
      return Forbid();
    }

    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.EorIotDevicePart,
      eorIotDevice =>
      {
        eorIotDevice.Controls.RunState = EorRunState.Started;
      }
    );
    await _contentManager.UpdateAsync(eorIotDevice);

    if (returnUrl is not null)
    {
      return Redirect(returnUrl);
    }

    return RedirectToAction(
      nameof(EorIotDeviceController.Detail),
      nameof(EorIotDeviceController),
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

    var eorIotDevice = contentItem.AsContent<EorIotDeviceItem>();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    if (
      !await _authorizationService.AuthorizeControlAsync(
        User,
        orchardCoreUser,
        eorIotDevice
      )
    )
    {
      return Forbid();
    }

    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.EorIotDevicePart,
      eorIotDevice =>
      {
        eorIotDevice.Controls.RunState = EorRunState.Stopped;
      }
    );
    await _contentManager.UpdateAsync(eorIotDevice);

    if (returnUrl is not null)
    {
      return Redirect(returnUrl);
    }

    return RedirectToAction(
      nameof(EorIotDeviceController.Detail),
      nameof(EorIotDeviceController),
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

    var eorIotDevice = contentItem.AsContent<EorIotDeviceItem>();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    if (
      !await _authorizationService.AuthorizeControlAsync(
        User,
        orchardCoreUser,
        eorIotDevice
      )
    )
    {
      return Forbid();
    }

    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.EorIotDevicePart,
      eorIotDevice =>
      {
        eorIotDevice.Controls.ResetState = EorResetState.ShouldntReset;
        eorIotDevice.Controls.RunState = EorRunState.Stopped;
      }
    );
    await _contentManager.UpdateAsync(eorIotDevice);

    if (returnUrl is not null)
    {
      return Redirect(returnUrl);
    }

    return RedirectToAction(
      nameof(EorIotDeviceController.Detail),
      nameof(EorIotDeviceController),
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

    var eorIotDevice = contentItem.AsContent<EorIotDeviceItem>();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    if (
      !await _authorizationService.AuthorizeControlAsync(
        User,
        orchardCoreUser,
        eorIotDevice
      )
    )
    {
      return Forbid();
    }

    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.EorIotDevicePart,
      eorIotDevice =>
      {
        eorIotDevice.Controls.Mode = mode;
      }
    );
    await _contentManager.UpdateAsync(eorIotDevice);

    if (returnUrl is not null)
    {
      return Redirect(returnUrl);
    }

    return RedirectToAction(
      nameof(EorIotDeviceController.Detail),
      nameof(EorIotDeviceController),
      new { contentItemId }
    );
  }

  public EorIotDeviceControlsController(
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
