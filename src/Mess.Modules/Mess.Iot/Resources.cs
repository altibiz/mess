using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Mess.Iot;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  static Resources()
  {
    _manifest = new ResourceManifest();
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }

  private static readonly ResourceManifest _manifest;
}
