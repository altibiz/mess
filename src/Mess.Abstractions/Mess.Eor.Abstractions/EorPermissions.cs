using OrchardCore.Security.Permissions;

namespace Mess.Eor.Abstractions;

public class EorPermissions : IPermissionProvider
{
  public static readonly Permission ManageEorMeasurementDevice = new Permission(
    "ManageEorMeasurementDevice",
    "Manage EOR measurement devices"
  );

  public static readonly Permission ViewEorMeasurementDevice = new Permission(
    "ViewEorMeasurementDevice",
    "View EOR measurement devices"
  );

  public Task<IEnumerable<Permission>> GetPermissionsAsync()
  {
    return Task.FromResult(
      new[]
      {
        ManageEorMeasurementDevice,
        ViewEorMeasurementDevice
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
          ManageEorMeasurementDevice,
          ViewEorMeasurementDevice
        }
      }
    };
  }
}
