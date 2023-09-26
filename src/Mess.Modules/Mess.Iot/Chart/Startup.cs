using Mess.Chart.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Mess.Iot.Chart;

[RequireFeatures("Mess.Chart")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddChartProvider<EgaugeChartProvider>();
  }
}
