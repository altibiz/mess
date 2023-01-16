using Mess.Chart.Abstractions.Extensions.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Mess.Timeseries.Chart;

[RequireFeatures("Mess.Chart")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.RegisterChartProvider<TimeseriesChartProvider>();
  }
}
