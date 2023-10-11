using OrchardCore.Security.Permissions;

namespace Mess.Ozds.Abstractions;

public class Permissions : IPermissionProvider
{
  public static readonly Permission ListMeasurementDevices = new Permission(
    "ListMeasurementDevices",
    "Listing measurement devices"
  );

  public static readonly Permission ListDistributionSystemOperators =
    new Permission(
      "ListDistributionSystemOperators",
      "Listing distribution system operators"
    );

  public static readonly Permission ListClosedDistributionSystems =
    new Permission(
      "ListClosedDistributionSystems",
      "Listing closed distribution systems"
    );

  public static readonly Permission ListDistributionSystemUnits =
    new Permission(
      "ListDistributionSystemUnits",
      "Listing distribution system units"
    );

  public Task<IEnumerable<Permission>> GetPermissionsAsync()
  {
    return Task.FromResult(
      new[]
      {
        ListMeasurementDevices,
        ListDistributionSystemOperators,
        ListClosedDistributionSystems,
        ListDistributionSystemUnits,
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
          ListMeasurementDevices,
          ListDistributionSystemOperators,
          ListClosedDistributionSystems,
          ListDistributionSystemUnits,
        }
      },
    };
  }
}
