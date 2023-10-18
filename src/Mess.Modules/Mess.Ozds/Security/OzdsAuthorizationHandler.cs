using Microsoft.AspNetCore.Authorization;
using OrchardCore.Security;

namespace Mess.Ozds.Security;

public class OzdsAuthorizationHandler
  : AuthorizationHandler<PermissionRequirement>
{
  protected override async Task HandleRequirementAsync(
    AuthorizationHandlerContext context,
    PermissionRequirement requirement
  ) { }
}
