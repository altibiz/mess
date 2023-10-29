using OrchardCore.Security.Permissions;

namespace Mess.Iot.Abstractions;

public class Permissions : IPermissionProvider
{
  public static readonly Permission ListIotDevices =
    new("ListIotDevices", "Listing IOT devices");

  public Task<IEnumerable<Permission>> GetPermissionsAsync()
  {
    return Task.FromResult(new[] { ListIotDevices, }.AsEnumerable());
  }

  public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
  {
    return new[]
    {
      new PermissionStereotype
      {
        Name = "Administrator",
        Permissions = new[] { ListIotDevices, }
      },
    };
  }
}
