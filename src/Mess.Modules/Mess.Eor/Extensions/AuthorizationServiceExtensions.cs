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
    EorIotDeviceItem contentItem
  )
  {
    return await authorizationService.AuthorizeAsync(
             claimsPrincipal,
             CommonPermissions.ViewContent,
             contentItem.Inner
           )
           || await authorizationService.AuthorizeAsync(
             claimsPrincipal,
             CommonPermissions.ViewOwnContent,
             contentItem.Inner
           )
           || (
             await authorizationService.AuthorizeAsync(
               claimsPrincipal,
               EorPermissions.ViewOwnedEorIotDevices
             )
             && contentItem.EorIotDevicePart.Value.Owner.UserIds
               .FirstOrDefault()
             == user.UserId
           );
  }

  public static async Task<bool> AuthorizeControlAsync(
    this IAuthorizationService authorizationService,
    ClaimsPrincipal claimsPrincipal,
    User user,
    EorIotDeviceItem contentItem
  )
  {
    return await authorizationService.AuthorizeAsync(
             claimsPrincipal,
             EorPermissions.ControlEorIotDevice,
             contentItem.Inner
           )
           || (
             await authorizationService.AuthorizeAsync(
               claimsPrincipal,
               EorPermissions.ControlOwnEorIotDevice
             )
             && contentItem.Inner.Owner == user.UserId
           )
           || (
             await authorizationService.AuthorizeAsync(
               claimsPrincipal,
               EorPermissions.ControlOwnedEorIotDevice
             )
             && contentItem.EorIotDevicePart.Value.Owner.UserIds
               .FirstOrDefault()
             == user.UserId
           );
  }
}
