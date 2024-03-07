using OrchardCore.ContentManagement.Records;
using OrchardCore.Users.Models;
using YesSql;
using YesSql.Indexes;
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

  public static IQuery<T> LatestPublished<T>(
    this IQuery<T> query
  ) where T : class? =>
    query.With<ContentItemIndex>(
      index => index.Published && index.Latest
    );

  public static IQuery<T, ContentItemIndex> LatestPublished<T, U>(
    this IQuery<T, U> query
  ) where T : class?
    where U : IIndex =>
    query.With<ContentItemIndex>(
      index => index.Published && index.Latest
    );
}
