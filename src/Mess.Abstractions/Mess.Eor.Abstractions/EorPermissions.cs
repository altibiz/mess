using OrchardCore.Security.Permissions;

namespace Mess.Eor.Abstractions;

public class EorPermissions : IPermissionProvider
{
  public static readonly Permission ControlEorIotDevice = new Permission(
    "Control_EorIotDevice",
    "Control EOR measurement devices"
  );

  public static readonly Permission ControlOwnEorIotDevice = new Permission(
    "ControlOwn_EorIotDevice",
    "Control EOR measurement devices"
  );

  public static readonly Permission ControlOwnedEorIotDevice = new Permission(
    "ControlOwned_EorIotDevice",
    "Control owned EOR measurement devices"
  );

  public static readonly Permission ViewOwnedEorIotDevices = new Permission(
    "ViewOwned_EorIotDevice",
    "View owned EOR measurement devices"
  );

  public Task<IEnumerable<Permission>> GetPermissionsAsync()
  {
    return Task.FromResult(
      new[]
      {
        ControlEorIotDevice,
        ControlOwnEorIotDevice,
        ControlOwnedEorIotDevice,
        ViewOwnedEorIotDevices,
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
          ControlEorIotDevice,
          ControlOwnEorIotDevice,
          ControlOwnedEorIotDevice,
          ViewOwnedEorIotDevices,
        }
      }
    };
  }
}
