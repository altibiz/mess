using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace Mess.Chart;

public class Resources : IConfigureOptions<ResourceManagementOptions>
{
  private static readonly ResourceManifest _manifest;

  static Resources()
  {
    _manifest = new ResourceManifest();

    _manifest
      .DefineScript("mess-chart")
      .SetUrl("~/Mess.Chart/assets/scripts/chart.js");

    _manifest
      .DefineStyle("mess-chart")
      .SetUrl("~/Mess.Chart/assets/styles/chart.css");
  }

  public void Configure(ResourceManagementOptions options)
  {
    options.ResourceManifests.Add(_manifest);
  }
}
