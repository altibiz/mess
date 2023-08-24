using System.Security.Claims;
using Mess.Eor.Abstractions;
using Mess.Eor.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using OrchardCore.Contents;
using OrchardCore.Users.Models;

namespace Mess.Eor.Extensions;

public static class AuthorizationServiceExtensions
{
  public static async Task<bool> AuthorizeViewAsync(
    this IAuthorizationService authorizationService,
    ClaimsPrincipal claimsPrincipal,
    User user,
    EorMeasurementDeviceItem eorMeasurementDevice
  )
  {
    return await authorizationService.AuthorizeAsync(
        claimsPrincipal,
        CommonPermissions.ViewContent,
        eorMeasurementDevice.Inner
      )
      || (
        await authorizationService.AuthorizeAsync(
          claimsPrincipal,
          CommonPermissions.ViewOwnContent,
          eorMeasurementDevice.Inner
        )
      )
      || (
        await authorizationService.AuthorizeAsync(
          claimsPrincipal,
          EorPermissions.ViewOwnedEorMeasurementDevices
        )
        && eorMeasurementDevice.EorMeasurementDevicePart.Value.Owner.UserIds.FirstOrDefault()
          == user.UserId
      );
  }

  public static async Task<bool> AuthorizeControlAsync(
    this IAuthorizationService authorizationService,
    ClaimsPrincipal claimsPrincipal,
    User user,
    EorMeasurementDeviceItem eorMeasurementDevice
  )
  {
    return await authorizationService.AuthorizeAsync(
        claimsPrincipal,
        EorPermissions.ControlEorMeasurementDevice,
        eorMeasurementDevice.Inner
      )
      || (
        await authorizationService.AuthorizeAsync(
          claimsPrincipal,
          EorPermissions.ControlOwnEorMeasurementDevice
        )
        && eorMeasurementDevice.Inner.Owner == user.UserId
      )
      || (
        await authorizationService.AuthorizeAsync(
          claimsPrincipal,
          EorPermissions.ControlOwnedEorMeasurementDevice
        )
        && eorMeasurementDevice.EorMeasurementDevicePart.Value.Owner.UserIds.FirstOrDefault()
          == user.UserId
      );
  }
}
