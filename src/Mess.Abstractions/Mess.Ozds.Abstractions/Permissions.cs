using OrchardCore.Security.Permissions;

namespace Mess.Ozds.Abstractions;

public class Permissions : IPermissionProvider
{
  public static readonly Permission ListIotDevices =
    new("ListIotDevices", "Listing measurement devices");

  public static readonly Permission ListDistributionSystemOperators =
    new(
      "ListDistributionSystemOperators",
      "Listing distribution system operators"
    );

  public static readonly Permission ListClosedDistributionSystems =
    new("ListClosedDistributionSystems", "Listing closed distribution systems");

  public static readonly Permission ListDistributionSystemUnits =
    new("ListDistributionSystemUnits", "Listing distribution system units");

  public Task<IEnumerable<Permission>> GetPermissionsAsync()
  {
    return Task.FromResult(
      new[]
      {
        ListIotDevices,
        ListDistributionSystemOperators,
        ListClosedDistributionSystems,
        ListDistributionSystemUnits
      }.AsEnumerable()
    );
  }

  public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
  {
    return new[]
    {
      new PermissionStereotype
      {
        Name = "Administrator",
        Permissions = new[]
        {
          ListIotDevices,
          ListDistributionSystemOperators,
          ListClosedDistributionSystems,
          ListDistributionSystemUnits
        }
      }
    };
  }
}
