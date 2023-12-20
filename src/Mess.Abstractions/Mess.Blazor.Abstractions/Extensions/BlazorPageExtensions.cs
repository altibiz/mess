using Mess.Blazor.Abstractions.Components;
using Mess.Cms.Extensions.OrchardCore;
using Microsoft.AspNetCore.Mvc.Razor;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;

namespace Mess.Blazor.Abstractions.Extensions;

// TODO: no throwing

public static class BlazorPageExtensions
{
  public static async Task<User> GetAuthenticatedOrchardCoreUserAsync(
    this PageComponentBase page
  )
  {
    var userService =
      page.ServiceProvider.GetRequiredService<IUserService>();

    var user = await userService.GetAuthenticatedOrchardCoreUserAsync(
      page.HttpContext.User
    ) ?? throw new UnauthorizedAccessException();

    return user;
  }
}
