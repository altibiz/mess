using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Mess.Chart;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  private static ResourceManifest _manifest;

  static Resources()
  {
    _manifest = new ResourceManifest();

    _manifest
      .DefineScript("mess-chart")
      .SetUrl(
        "~/Mess.Chart/assets/chart/chart.min.js",
        "~/Mess.Chart/assets/chart/chart.js"
      );
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }
}
