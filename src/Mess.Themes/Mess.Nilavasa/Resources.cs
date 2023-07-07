using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Mess.Nilavasa;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  static Resources()
  {
    _manifest = new ResourceManifest();

    _manifest
      .DefineScript("mess-nilavasa")
      .SetUrl("~/Mess.Nilavasa/assets/scripts/nilavasa.js");

    _manifest
      .DefineStyle("mess-nilavasa")
      .SetUrl("~/Mess.Nilavasa/assets/styles/nilavasa.css");
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }

  private static ResourceManifest _manifest;
}
