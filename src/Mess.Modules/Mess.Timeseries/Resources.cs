using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Mess.Timeseries;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  private static ResourceManifest _manifest;

  static Resources()
  {
    _manifest = new ResourceManifest();
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }
}
