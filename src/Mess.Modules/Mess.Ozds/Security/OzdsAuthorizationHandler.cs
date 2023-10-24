using Microsoft.AspNetCore.Authorization;
using OrchardCore.Security;

namespace Mess.Ozds.Security;

public class OzdsAuthorizationHandler
  : AuthorizationHandler<PermissionRequirement>
{
  // TODO: implement like in controllers
  protected override async Task HandleRequirementAsync(
    AuthorizationHandlerContext context,
    PermissionRequirement requirement
  ) { }
}
