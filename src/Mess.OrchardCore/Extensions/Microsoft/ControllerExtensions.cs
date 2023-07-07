using Mess.OrchardCore.Extensions.OrchardCore;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;

namespace Mess.OrchardCore.Extensions.Microsoft;

public static class ControllerExtensions
{
  public static async Task<User> GetAuthenticatedOrchardCoreUserAsync(
    this Controller controller
  )
  {
    var userService =
      controller.HttpContext.RequestServices.GetRequiredService<IUserService>();
    var user = await userService.GetAuthenticatedOrchardCoreUserAsync(
      controller.User
    );
    if (user == null)
    {
      throw new UnauthorizedAccessException();
    }

    return user;
  }
}
