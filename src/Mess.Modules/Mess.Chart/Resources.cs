using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Mess.Charts;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  private static ResourceManifest _manifest;

  static Resources()
  {
    _manifest = new ResourceManifest();

    _manifest
      .DefineScript("mess-charts")
      .SetUrl(
        "~/Mess.Charts/assets/charts/charts.min.js",
        "~/Mess.Charts/assets/charts/charts.js"
      );
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }
}
