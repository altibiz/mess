using OrchardCore.Users.Models;
using ISession = YesSql.ISession;

namespace Mess.Cms.Extensions.OrchardCore;

public static class ISessionExtensions
{
  public static async Task<User> CreateDevUserAsync(
    this ISession session,
    string id,
    string userName,
    params string[]? roleNames
  )
  {
    // TODO: password hash?
    var user = new User
    {
      UserId = id,
      UserName = userName,
      Email = $"{userName.ToLowerInvariant()}@dev.com",
      RoleNames = roleNames is null or { Length: 0 }
        ? new[] { userName }
        : roleNames
    };

    session.Save(user);

    return user;
  }
}
