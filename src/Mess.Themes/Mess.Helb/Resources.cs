using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Mess.Helb;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  private static readonly ResourceManifest _manifest;

  static Resources()
  {
    _manifest = new ResourceManifest();

    _manifest
      .DefineStyle("mess-helb")
      .SetUrl("~/Mess.Helb/assets/styles/helb.css");
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }
}
