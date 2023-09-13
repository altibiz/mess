using Microsoft.AspNetCore.Identity;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;

namespace Mess.Ozds;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    // await _roleManager.CreateAsync(
    //   new Role
    //   {
    //     NormalizedRoleName = "EorMeasurementDeviceAdmin",
    //     RoleName = "EOR measurement device administrator",
    //     RoleDescription = "Administrator of an EOR measurement devices.",
    //     RoleClaims = new()
    //     {
    //       new RoleClaim
    //       {
    //         ClaimType = Permission.ClaimType,
    //         ClaimValue = "AccessAdminPanel"
    //       },
    //       new RoleClaim
    //       {
    //         ClaimType = Permission.ClaimType,
    //         ClaimValue = "View Users"
    //       },
    //       new RoleClaim
    //       {
    //         ClaimType = Permission.ClaimType,
    //         ClaimValue = "ManageUsersInRole_EOR measurement device owner"
    //       },
    //       new RoleClaim
    //       {
    //         ClaimType = Permission.ClaimType,
    //         ClaimValue = "AssignRole_EOR measurement device owner"
    //       },
    //       new RoleClaim
    //       {
    //         ClaimType = Permission.ClaimType,
    //         ClaimValue = "ViewOwn_EorMeasurementDevice"
    //       },
    //       new RoleClaim
    //       {
    //         ClaimType = Permission.ClaimType,
    //         ClaimValue = "ControlOwn_EorMeasurementDevice"
    //       },
    //       new RoleClaim
    //       {
    //         ClaimType = Permission.ClaimType,
    //         ClaimValue = "PublishOwn_EorMeasurementDevice"
    //       },
    //       new RoleClaim
    //       {
    //         ClaimType = Permission.ClaimType,
    //         ClaimValue = "EditOwn_EorMeasurementDevice"
    //       },
    //       new RoleClaim
    //       {
    //         ClaimType = Permission.ClaimType,
    //         ClaimValue = "DeleteOwn_EorMeasurementDevice"
    //       },
    //     }
    //   }
    // );

    // await _roleManager.CreateAsync(
    //   new Role
    //   {
    //     NormalizedRoleName = "EorMeasurementDeviceOwner",
    //     RoleName = "EOR measurement device owner",
    //     RoleDescription = "Owner of an EOR measurement devices.",
    //     RoleClaims = new()
    //     {
    //       new RoleClaim
    //       {
    //         ClaimType = Permission.ClaimType,
    //         ClaimValue = "ViewOwned_EorMeasurementDevice"
    //       },
    //       new RoleClaim
    //       {
    //         ClaimType = Permission.ClaimType,
    //         ClaimValue = "ControlOwned_EorMeasurementDevice"
    //       }
    //     }
    //   }
    // );

    // var ownerId = "OwnerId";
    // await _userService.CreateUserAsync(
    //   new User
    //   {
    //     UserId = ownerId,
    //     UserName = "Owner",
    //     Email = "owner@dev.com",
    //     RoleNames = new[] { "EOR measurement device owner" }
    //   },
    //   "Owner123!",
    //   (_, _) => { }
    // );

    // var adminId = "AdminId";
    // await _userService.CreateUserAsync(
    //   new User
    //   {
    //     UserId = adminId,
    //     UserName = "Admin",
    //     Email = "admin@dev.com",
    //     RoleNames = new[] { "EOR measurement device administrator" }
    //   },
    //   "Admin123!",
    //   (_, _) => { }
    // );

    return 1;
  }

  public Migrations(
    IContentDefinitionManager contentDefinitionManager,
    IRecipeMigrator recipeMigrator,
    RoleManager<IRole> roleManager,
    IUserService userService
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    _recipeMigrator = recipeMigrator;
    _roleManager = roleManager;
    _userService = userService;
  }

  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IRecipeMigrator _recipeMigrator;
  private readonly RoleManager<IRole> _roleManager;
  private readonly IUserService _userService;
}
