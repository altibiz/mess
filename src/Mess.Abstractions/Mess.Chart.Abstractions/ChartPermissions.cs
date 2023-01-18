using OrchardCore.Security.Permissions;

namespace Mess.Chart.Abstractions;

public class ChartPermissions : IPermissionProvider
{
  public static readonly Permission ManageChart = new Permission(
    "ManageChart",
    "Manage charts"
  );

  public Task<IEnumerable<Permission>> GetPermissionsAsync()
  {
    return Task.FromResult(new[] { ManageChart }.AsEnumerable());
  }

  public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
  {
    return new[]
    {
      new PermissionStereotype
      {
        Name = "Administrator",
        Permissions = new[] { ManageChart }
      }
    };
  }
}
