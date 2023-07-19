using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Mess.Forttech;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  static Resources()
  {
    _manifest = new ResourceManifest();

    _manifest
      .DefineStyle("mess-forttech")
      .SetUrl("~/Mess.Forttech/assets/styles/forttech.css");

    _manifest
      .DefineScript("mess-forttech")
      .SetUrl("~/Mess.Forttech/assets/scripts/forttech.js");
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }

  private static ResourceManifest _manifest;
}
