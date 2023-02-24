using Mess.Chart.Abstractions.Extensions.Microsoft;
using Mess.MeasurementDevice.Chart.Providers;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Mess.MeasurementDevice.Chart;

[RequireFeatures("Mess.Chart")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.RegisterChartProvider<EgaugeChartDataProvider>();
  }
}