using System.Security.Claims;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using ISession = YesSql.ISession;

namespace Mess.OrchardCore.Extensions.OrchardCore;

public static class IUserServiceExtensions
{
  public static async Task<User> GetAuthenticatedOrchardCoreUserAsync(
    this IUserService userService,
    ClaimsPrincipal claimsPrincipal
  )
  {
    var orchardUser = await userService.GetAuthenticatedUserAsync(claimsPrincipal);

    return orchardUser is not User user ? throw new UnauthorizedAccessException() : user;
  }

  public static async Task<User> CreateDevUserAsync(
    this IUserService userService,
    string id,
    string userName,
    params string[]? roleNames
  )
  {
    var user = new User
    {
      UserId = id,
      UserName = userName,
      Email = $"{userName.ToLowerInvariant()}@dev.com",
      RoleNames = roleNames is null or { Length: 0 }
        ? new[] { userName }
        : roleNames
    };

    await userService.CreateUserAsync(user, $"{userName}123!", (_, _) => { });

    return user;
  }
}
