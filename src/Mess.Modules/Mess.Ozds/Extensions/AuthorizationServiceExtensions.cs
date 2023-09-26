using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using OrchardCore.ContentManagement;
using OrchardCore.Contents;
using OrchardCore.Users.Models;

namespace Mess.Ozds.Extensions;

public static class AuthorizationServiceExtensions
{
  public static async Task<bool> AuthorizeViewAsync(
    this IAuthorizationService authorizationService,
    ClaimsPrincipal claimsPrincipal,
    User user,
    ContentItem contentItem
  )
  {
    return await authorizationService.AuthorizeAsync(
        claimsPrincipal,
        CommonPermissions.ViewContent,
        contentItem
      )
      || (
        await authorizationService.AuthorizeAsync(
          claimsPrincipal,
          CommonPermissions.ViewOwnContent,
          contentItem
        )
      );
  }
}
