using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Mess.Enms;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  private static readonly ResourceManifest _manifest;

  static Resources()
  {
    _manifest = new ResourceManifest();
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }
}
