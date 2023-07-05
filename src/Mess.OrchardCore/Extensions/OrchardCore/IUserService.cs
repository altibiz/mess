using System.Security.Claims;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;

namespace Mess.OrchardCore.Extensions.OrchardCore;

public static class IUserServiceExtensions
{
  public static async Task<User> GetAuthenticatedOrchardCoreUserAsync(
    this IUserService userService,
    ClaimsPrincipal user
  )
  {
    var orchardUser = await userService.GetAuthenticatedUserAsync(user) as User;
    if (orchardUser == null)
    {
      throw new UnauthorizedAccessException();
    }

    return orchardUser;
  }
}
