using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Mess.Helb;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  static Resources()
  {
    _manifest = new ResourceManifest();

    _manifest
      .DefineStyle("mess-helb")
      .SetUrl("~/Mess.Helb/assets/styles/helb.css");

    _manifest
      .DefineScript("mess-helb")
      .SetUrl("~/Mess.Helb/assets/scripts/helb.js");
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }

  private static readonly ResourceManifest _manifest;
}
