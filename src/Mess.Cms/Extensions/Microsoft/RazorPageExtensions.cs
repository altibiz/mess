using Mess.Cms.Extensions.OrchardCore;
using Microsoft.AspNetCore.Mvc.Razor;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;

namespace Mess.Cms.Extensions.Microsoft;

// TODO: no throwing

public static class RazorPageExtensions
{
  public static async Task<User> GetAuthenticatedOrchardCoreUserAsync(
    this RazorPage page
  )
  {
    var userService =
      page.Context.RequestServices.GetRequiredService<IUserService>();

    var user = await userService.GetAuthenticatedOrchardCoreUserAsync(
      page.User
    ) ?? throw new UnauthorizedAccessException();

    return user;
  }
}
