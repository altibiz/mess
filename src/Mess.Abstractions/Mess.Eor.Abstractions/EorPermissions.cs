using OrchardCore.Security.Permissions;

namespace Mess.Eor.Abstractions;

public class EorPermissions : IPermissionProvider
{
  public static readonly Permission ControlEorMeasurementDevice =
    new Permission(
      "Control_EorMeasurementDevice",
      "Control EOR measurement devices"
    );

  public static readonly Permission ControlOwnEorMeasurementDevice =
    new Permission(
      "ControlOwn_EorMeasurementDevice",
      "Control EOR measurement devices"
    );

  public static readonly Permission ControlOwnedEorMeasurementDevice =
    new Permission(
      "ControlOwned_EorMeasurementDevice",
      "Control owned EOR measurement devices"
    );

  public static readonly Permission ViewOwnedEorMeasurementDevices =
    new Permission(
      "ViewOwned_EorMeasurementDevice",
      "View owned EOR measurement devices"
    );

  public Task<IEnumerable<Permission>> GetPermissionsAsync()
  {
    return Task.FromResult(
      new[]
      {
        ControlEorMeasurementDevice,
        ControlOwnEorMeasurementDevice,
        ControlOwnedEorMeasurementDevice,
        ViewOwnedEorMeasurementDevices,
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
          ControlEorMeasurementDevice,
          ControlOwnEorMeasurementDevice,
          ControlOwnedEorMeasurementDevice,
          ViewOwnedEorMeasurementDevices,
        }
      }
    };
  }
}
