using Mess.OrchardCore.Extensions.OrchardCore;
using Microsoft.AspNetCore.Identity;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;
using OrchardCore.Users.Services;

namespace Mess.Ozds;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "Operator",
        RoleName = "Operator",
        RoleDescription = "Operator of closed distribution systems.",
        RoleClaims = new()
        {
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AccessAdminPanel"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "View Users"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ManageUsersInRole_Owner"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AssignRole_Owner"
          },
        }
      }
    );

    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "Owner",
        RoleName = "Owner",
        RoleDescription = "Owner of a closed distribution system.",
        RoleClaims = new()
        {
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AccessAdminPanel"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "View Users"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ManageUsersInRole_Unit"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AssignRole_Unit"
          },
        }
      }
    );

    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "Unit",
        RoleName = "Unit",
        RoleDescription = "Unit in a closed distribution system.",
      }
    );

    var operatorId = "OperatorId";
    await _userService.CreateDevUserAsync(operatorId, "Operator", "Operator");

    var ownerId = "OwnerId";
    await _userService.CreateDevUserAsync(ownerId, "Owner", "Owner");

    var unitId = "UnitId";
    await _userService.CreateDevUserAsync(unitId, "Unit", "Unit");

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
