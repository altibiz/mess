using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Mess.Eor;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  private static readonly ResourceManifest _manifest;

  static Resources()
  {
    _manifest = new ResourceManifest();

    _manifest
      .DefineScript("mess-eor")
      .SetUrl("~/Mess.Eor/assets/scripts/eor.js");
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }
}
